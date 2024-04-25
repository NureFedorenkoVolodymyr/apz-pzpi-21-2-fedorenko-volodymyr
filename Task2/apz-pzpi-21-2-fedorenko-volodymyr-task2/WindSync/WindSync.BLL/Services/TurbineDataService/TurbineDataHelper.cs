using WindSync.BLL.Dtos;
using WindSync.BLL.Services.AlertService;
using WindSync.Core.Enums;
using WindSync.Core.Models;
using WindSync.DAL.Repositories.TurbineRepository;

namespace WindSync.BLL.Services.TurbineDataService;

public class TurbineDataHelper : ITurbineDataHelper
{
    private readonly ITurbineRepository _turbineRepository;
    private readonly IAlertService _alertService;

    public TurbineDataHelper(ITurbineRepository turbineRepository, IAlertService alertService)
    {
        _turbineRepository = turbineRepository;
        _alertService = alertService;
    }

    public async Task<TurbineStatus> UpdateTurbineStatusNewAsync(Turbine turbine, TurbineData currentData)
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
                await SendAlert("Speed of wind is below cut-in speed. Turbine status is changed to Idle.",
                    AlertStatus.Informational,
                    turbine);
            }
        }
        else if (IsShutDownWindSpeed(turbine, currentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Idle);

            if (result)
            {
                resultStatus = TurbineStatus.Idle;
                await SendAlert("Speed of wind is greater than shutdown speed. Turbine status is changed to Idle.",
                    AlertStatus.Warning,
                    turbine);
            }
        }
        else if (IsCutInWindSpeed(turbine, currentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Operational);

            if (result)
            {
                resultStatus = TurbineStatus.Operational;
                await SendAlert("Speed of wind is in cut-in speed. Turbine status is unchanged.",
                    AlertStatus.Informational,
                    turbine);
            }
        }
        else if (IsRatedWindSpeed(turbine, currentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Operational);

            if (result)
            {
                resultStatus = TurbineStatus.Operational;
                await SendAlert("Speed of wind is in rated speed. Turbine status is unchanged. Turbine power output is restricted.",
                    AlertStatus.Informational,
                    turbine);
            }
        }

        return resultStatus;
    }

    public async Task<TurbineStatus> UpdateTurbineStatusAsync(Turbine turbine, TurbineData currentData, TurbineData recentData)
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
                await SendAlert("Speed of wind is below cut-in speed. Turbine status is changed to Idle.",
                    AlertStatus.Informational,
                    turbine);
            }
        }
        else if (IsShutDownWindSpeed(turbine, currentData.WindSpeed)
            && !IsShutDownWindSpeed(turbine, recentData.WindSpeed))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Idle);

            if (result)
            {
                resultStatus = TurbineStatus.Idle;
                await SendAlert("Speed of wind is greater than shutdown speed. Turbine status is changed to Idle.",
                    AlertStatus.Warning,
                    turbine);
            }
        }
        else if (IsCutInWindSpeed(turbine, currentData.WindSpeed)
            && IsRatedWindSpeed(turbine, recentData.WindSpeed))
        {
            await SendAlert("Speed of wind is in cut-in speed. Turbine status is unchanged.",
                    AlertStatus.Informational,
                    turbine);
        }
        else if (IsRatedWindSpeed(turbine, currentData.WindSpeed)
            && IsCutInWindSpeed(turbine, recentData.WindSpeed))
        {
            await SendAlert("Speed of wind is in rated speed. Turbine status is unchanged. Turbine power output is restricted.",
                    AlertStatus.Informational,
                    turbine);
        }
        else if ((IsCutInWindSpeed(turbine, currentData.WindSpeed) || IsRatedWindSpeed(turbine, currentData.WindSpeed))
            && (IsBelowCutInWindSpeed(turbine, recentData.WindSpeed) || IsShutDownWindSpeed(turbine, recentData.WindSpeed)))
        {
            var result = await _turbineRepository.ChangeTurbineStatusAsync(turbine.Id, TurbineStatus.Operational);

            if (result)
            {
                resultStatus = TurbineStatus.Operational;
                await SendAlert("Speed of wind is in cut-in/rated speed. Turbine status is changed from Idle to Operational.",
                    AlertStatus.Resolved,
                    turbine);
            }
        }

        return resultStatus;
    }

    public bool IsBelowCutInWindSpeed(Turbine turbine, double windSpeed) => windSpeed < turbine.CutInWindSpeed;

    public bool IsCutInWindSpeed(Turbine turbine, double windSpeed) => windSpeed >= turbine.CutInWindSpeed && windSpeed < turbine.RatedWindSpeed;

    public bool IsRatedWindSpeed(Turbine turbine, double windSpeed) => windSpeed >= turbine.RatedWindSpeed && windSpeed < turbine.ShutDownWindSpeed;

    public bool IsShutDownWindSpeed(Turbine turbine, double windSpeed) => windSpeed >= turbine.ShutDownWindSpeed;

    public async Task SendAlert(string message, AlertStatus status, Turbine turbine)
    {
        var alert = new AlertDto()
        {
            Message = $"Turbine: {turbine.Id}\nWindFarm: {turbine.WindFarmId}\n{message}",
            DateTime = DateTime.UtcNow,
            Status = status,
            UserId = turbine.WindFarm.UserId
        };
        await _alertService.AddAlertAsync(alert);
    }
}
