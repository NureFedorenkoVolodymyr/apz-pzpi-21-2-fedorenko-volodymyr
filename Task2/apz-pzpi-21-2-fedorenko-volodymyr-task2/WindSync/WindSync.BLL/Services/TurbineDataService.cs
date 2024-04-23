using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.Core.Utils;
using WindSync.DAL.Repositories;

namespace WindSync.BLL.Services;

public class TurbineDataService : ITurbineDataService
{
    private readonly ITurbineDataRepository _repository;
    private readonly ITurbineRepository _turbineRepository;
    private readonly IMapper _mapper;

    public TurbineDataService(ITurbineDataRepository repository, ITurbineRepository turbineRepository, IMapper mapper)
    {
        _repository = repository;
        _turbineRepository = turbineRepository;
        _mapper = mapper;
    }

    public async Task<bool> AddDataAsync(TurbineDataAddDto data)
    {
        var model = _mapper.Map<TurbineData>(data);
        var turbine = await _turbineRepository.GetTurbineByIdAsync(model.TurbineId);
        var recentData = await _turbineRepository.GetMostRecentTurbineDataAsync(model.TurbineId);

        // Calculate air density (p)
        // Formula: p = P / (R * T)
        // P - air pressure
        // R - gas constant
        // T - air temperature
        model.AirDensity = model.AirPressure / (Constants.GasConstantForAir * model.AirTemperature);

        // Calculate rated power (P)
        // Formula: P = 1/2 * p * A * V^3
        // p - air density
        // A - swept area
        // V - wind speed
        model.RatedPower = (model.AirDensity * turbine.SweptArea * Math.Pow(model.WindSpeed, 3)) / 2;

        // Calculate power output (Po)
        // Formula: Po = P * (t2 - t1)
        // P - rated power
        // t2 - current time
        // t1 - most recent record time
        model.PowerOutput = model.RatedPower * (model.DateTime - recentData.DateTime).TotalSeconds;

        return await _repository.AddDataAsync(model);
    }

    public async Task<bool> DeleteDataAsync(int dataId)
    {
        return await _repository.DeleteDataAsync(dataId);
    }
}
