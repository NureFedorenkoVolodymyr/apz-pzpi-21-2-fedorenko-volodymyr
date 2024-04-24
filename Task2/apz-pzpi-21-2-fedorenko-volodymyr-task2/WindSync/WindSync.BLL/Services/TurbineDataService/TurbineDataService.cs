using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services.AlertService;
using WindSync.Core.Enums;
using WindSync.Core.Models;
using WindSync.Core.Utils;
using WindSync.DAL.Repositories.TurbineDataRepository;
using WindSync.DAL.Repositories.TurbineRepository;

namespace WindSync.BLL.Services.TurbineDataService;

public class TurbineDataService : ITurbineDataService
{
    private readonly ITurbineDataRepository _turbineDataRepository;
    private readonly ITurbineRepository _turbineRepository;
    private readonly IAlertService _alertService;
    private readonly IMapper _mapper;

    public TurbineDataService(
        ITurbineDataRepository repository,
        ITurbineRepository turbineDataRepository,
        IAlertService alertService,
        IMapper mapper
        )
    {
        _turbineDataRepository = repository;
        _turbineRepository = turbineDataRepository;
        _alertService = alertService;
        _mapper = mapper;
    }

    public async Task<TurbineStatus> AddDataAsync(TurbineDataDto data)
    {
        var model = _mapper.Map<TurbineData>(data);
        var turbine = await _turbineRepository.GetTurbineByIdAsync(model.TurbineId);
        var recentData = await _turbineRepository.GetMostRecentTurbineDataAsync(model.TurbineId);

        // Calculate air pressure (P)
        // Formula: P = P0 * e ^ ((-u * g * h) / (R * T))
        // 
        model.AirPressure = Constants.SeaLevelPressure * Math.Exp(-Constants.MolarMassOfAir * Constants.AccelerationOfGravity * turbine.Altitude / (Constants.UniversalGasConstant * model.AirTemperature));

        // Calculate air density (p)
        // Formula: p = P / (R * T)
        // P - air pressure
        // R - gas constant
        // T - air temperature
        model.AirDensity = model.AirPressure / (Constants.GasConstantForAir * model.AirTemperature);

        // Calculate rated power and average rated power (P and Pavg)
        // Formula: P = 1/2 * p * A * V^3
        // p - air density
        // A - swept area
        // V - wind speed
        var currentWindSpeed = model.WindSpeed;
        if (IsBelowCutInWindSpeed(turbine, model.WindSpeed) || IsShutDownWindSpeed(turbine, model.WindSpeed))
            currentWindSpeed = 0;
        else if (IsRatedWindSpeed(turbine, model.WindSpeed))
            currentWindSpeed = turbine.RatedWindSpeed;

        model.RatedPower = model.AirDensity * turbine.SweptArea * Math.Pow(currentWindSpeed, 3) / 2 * turbine.Efficiency;

        if(recentData is not null)
        {
            var recentWindSpeed = recentData.WindSpeed;
            if (IsBelowCutInWindSpeed(turbine, recentData.WindSpeed) || IsShutDownWindSpeed(turbine, recentData.WindSpeed))
                recentWindSpeed = 0;
            else if (IsRatedWindSpeed(turbine, recentData.WindSpeed))
                recentWindSpeed = turbine.RatedWindSpeed;

            var avgAirDensity = (model.AirDensity + recentData.AirDensity) / 2;
            var avgWindSpeed = (currentWindSpeed + recentWindSpeed) / 2;
            var Pavg = avgAirDensity * turbine.SweptArea * Math.Pow(avgWindSpeed, 3) / 2 * turbine.Efficiency;

            // Calculate power output (Po)
            // Formula: Po = Pavg * (t2 - t1)
            // Pavg - average rated power
            // t2 - current time
            // t1 - most recent record time
            model.PowerOutput = Pavg * (model.DateTime - recentData.DateTime).TotalSeconds / 3600;
        }

        var addDataResult = await _turbineDataRepository.AddDataAsync(model);
        if (!addDataResult)
            return TurbineStatus.None;

        if(recentData is null)
            return await UpdateTurbineStatusNewAsync(turbine, model);

        return await UpdateTurbineStatusAsync(turbine, model, recentData);
    }

    public async Task<bool> DeleteDataAsync(int dataId)
    {
        return await _turbineDataRepository.DeleteDataAsync(dataId);
    }

    private async Task<TurbineStatus> UpdateTurbineStatusNewAsync(Turbine turbine, TurbineData currentData)
    {
        var resultStatus = turbine.Status;

        if (turbine.Status != TurbineStatus.Operational && turbine.Status != TurbineStatus.Idle)
            return resultStatus;

        if (IsBelowCutInWindSpeed(turbine, currentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Idle);

            if (result)
            {
                resultStatus = TurbineStatus.Idle;
                await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is below cut-in speed. Turbine status is changed to Idle.",
                    AlertStatus.Informational,
                    turbine.WindFarm.UserId);
            }
        }
        else if (IsShutDownWindSpeed(turbine, currentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Idle);

            if (result)
            {
                resultStatus = TurbineStatus.Idle;
                await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is greater than shutdown speed. Turbine status is changed to Idle.",
                    AlertStatus.Warning,
                    turbine.WindFarm.UserId);
            }
        }
        else if (IsCutInWindSpeed(turbine, currentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Operational);

            if(result)
            {
                resultStatus = TurbineStatus.Operational;
                await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is in cut-in speed. Turbine status is unchanged.",
                    AlertStatus.Informational,
                    turbine.WindFarm.UserId);
            }
        }
        else if (IsRatedWindSpeed(turbine, currentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Operational);

            if (result)
            {
                resultStatus = TurbineStatus.Operational;
                await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is in rated speed. Turbine status is unchanged. Turbine power output is restricted.",
                    AlertStatus.Informational,
                    turbine.WindFarm.UserId);
            }
        }

        return resultStatus;
    }

    private async Task<TurbineStatus> UpdateTurbineStatusAsync(Turbine turbine, TurbineData currentData, TurbineData recentData)
    {
        var resultStatus = turbine.Status;

        if (turbine.Status != TurbineStatus.Operational && turbine.Status != TurbineStatus.Idle)
            return resultStatus;

        if (IsBelowCutInWindSpeed(turbine, currentData.WindSpeed)
            && !IsBelowCutInWindSpeed(turbine, recentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Idle);

            if (result)
            {
                resultStatus = TurbineStatus.Idle;
                await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is below cut-in speed. Turbine status is changed to Idle.",
                    AlertStatus.Informational,
                    turbine.WindFarm.UserId);
            }
        }
        else if (IsShutDownWindSpeed(turbine, currentData.WindSpeed)
            && !IsShutDownWindSpeed(turbine, recentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Idle);

            if (result)
            {
                resultStatus = TurbineStatus.Idle;
                await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is greater than shutdown speed. Turbine status is changed to Idle.",
                    AlertStatus.Warning,
                    turbine.WindFarm.UserId);
            }
        }
        else if (IsCutInWindSpeed(turbine, currentData.WindSpeed)
            && IsRatedWindSpeed(turbine, recentData.WindSpeed))
        {
            await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is in cut-in speed. Turbine status is unchanged.",
                    AlertStatus.Informational,
                    turbine.WindFarm.UserId);
        }
        else if (IsRatedWindSpeed(turbine, currentData.WindSpeed)
            && IsCutInWindSpeed(turbine, recentData.WindSpeed))
        {
            await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is in rated speed. Turbine status is unchanged. Turbine power output is restricted.",
                    AlertStatus.Informational,
                    turbine.WindFarm.UserId);
        }
        else if ((IsCutInWindSpeed(turbine, currentData.WindSpeed) || IsRatedWindSpeed(turbine, currentData.WindSpeed))
            && (IsBelowCutInWindSpeed(turbine, recentData.WindSpeed) || IsShutDownWindSpeed(turbine, recentData.WindSpeed)))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Operational);

            if (result)
            {
                resultStatus = TurbineStatus.Operational;
                await SendAlert($"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\nSpeed of wind is in cut-in/rated speed. Turbine status is changed from Idle to Operational.",
                    AlertStatus.Resolved,
                    turbine.WindFarm.UserId);
            }
        }

        return resultStatus;
    }

    private bool IsBelowCutInWindSpeed(Turbine turbine, double windSpeed) => windSpeed < turbine.CutInWindSpeed;

    private bool IsCutInWindSpeed(Turbine turbine, double windSpeed) => windSpeed >= turbine.CutInWindSpeed && windSpeed < turbine.RatedWindSpeed;

    private bool IsRatedWindSpeed(Turbine turbine, double windSpeed) => windSpeed >= turbine.RatedWindSpeed && windSpeed < turbine.ShutDownWindSpeed;

    private bool IsShutDownWindSpeed(Turbine turbine, double windSpeed) => windSpeed >= turbine.ShutDownWindSpeed;

    private async Task SendAlert(string message, AlertStatus status, string userId)
    {
        var alert = new AlertDto()
        {
            Message = message,
            DateTime = DateTime.UtcNow,
            Status = status,
            UserId = userId,
        };
        await _alertService.AddAlertAsync(alert);
    }
}
