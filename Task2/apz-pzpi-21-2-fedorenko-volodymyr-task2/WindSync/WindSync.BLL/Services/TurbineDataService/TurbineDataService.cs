using AutoMapper;
using WindSync.BLL.Dtos;
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
    private readonly ITurbineDataHelper _turbineDataHelper;
    private readonly IMapper _mapper;

    public TurbineDataService(
        ITurbineDataRepository repository,
        ITurbineRepository turbineDataRepository,
        ITurbineDataHelper turbineDataHelper,
        IMapper mapper
        )
    {
        _turbineDataRepository = repository;
        _turbineRepository = turbineDataRepository;
        _turbineDataHelper = turbineDataHelper;
        _mapper = mapper;
    }

    public async Task<TurbineStatus> AddDataAsync(TurbineDataDto data)
    {
        var model = _mapper.Map<TurbineData>(data);
        var turbine = await _turbineRepository.GetTurbineByIdAsync(model.TurbineId);
        var recentData = await _turbineRepository.GetMostRecentTurbineDataAsync(model.TurbineId);

        // Calculate air pressure (P)
        // Formula: P = P0 * e ^ ((-u * g * h) / (R * T))
        // P0 - sea level air pressure
        // u - molar mass of air
        // g - acceleration of gravity
        // h - altitude
        // R - universal gas constant
        // T - air temperature
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
        if (_turbineDataHelper.IsBelowCutInWindSpeed(turbine, model.WindSpeed)
            || _turbineDataHelper.IsShutDownWindSpeed(turbine, model.WindSpeed))
            currentWindSpeed = 0;
        else if (_turbineDataHelper.IsRatedWindSpeed(turbine, model.WindSpeed))
            currentWindSpeed = turbine.RatedWindSpeed;

        model.RatedPower = model.AirDensity * turbine.SweptArea * Math.Pow(currentWindSpeed, 3) / 2 * turbine.Efficiency;

        if(recentData is not null)
        {
            var recentWindSpeed = recentData.WindSpeed;
            if (_turbineDataHelper.IsBelowCutInWindSpeed(turbine, recentData.WindSpeed)
                || _turbineDataHelper.IsShutDownWindSpeed(turbine, recentData.WindSpeed))
                recentWindSpeed = 0;
            else if (_turbineDataHelper.IsRatedWindSpeed(turbine, recentData.WindSpeed))
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
            return await _turbineDataHelper.UpdateTurbineStatusNewAsync(turbine, model);

        return await _turbineDataHelper.UpdateTurbineStatusAsync(turbine, model, recentData);
    }

    public async Task<bool> DeleteDataAsync(int dataId)
    {
        return await _turbineDataRepository.DeleteDataAsync(dataId);
    }
}
