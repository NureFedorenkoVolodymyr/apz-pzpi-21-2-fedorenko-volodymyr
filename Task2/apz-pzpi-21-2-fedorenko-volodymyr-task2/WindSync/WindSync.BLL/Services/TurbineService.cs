using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Enums;
using WindSync.Core.Models;
using WindSync.DAL.Repositories;

namespace WindSync.BLL.Services;

public class TurbineService : ITurbineService
{
    private readonly ITurbineRepository _turbineRepository;
    private readonly IMapper _mapper;

    public TurbineService(ITurbineRepository turbineRepository, IMapper mapper)
    {
        _turbineRepository = turbineRepository;
        _mapper = mapper;
    }

    public async Task<List<TurbineDto>> GetTurbinesByUserAsync(string userId)
    {
        var turbines = await _turbineRepository.GetTurbinesByUserAsync(userId);
        return _mapper.Map<List<TurbineDto>>(turbines);
    }

    public async Task<TurbineDto> GetTurbineByIdAsync(int turbineId)
    {
        var turbine = await _turbineRepository.GetTurbineByIdAsync(turbineId);
        return _mapper.Map<TurbineDto>(turbine);
    }

    public async Task<bool> AddTurbineAsync(TurbineDto turbineDto)
    {
        var turbine = _mapper.Map<Turbine>(turbineDto);
        return await _turbineRepository.AddTurbineAsync(turbine);
    }

    public async Task<bool> UpdateTurbineAsync(TurbineDto turbineDto)
    {
        var turbine = _mapper.Map<Turbine>(turbineDto);
        return await _turbineRepository.UpdateTurbineAsync(turbine);
    }

    public async Task<bool> DeleteTurbineAsync(int turbineId)
    {
        return await _turbineRepository.DeleteTurbineAsync(turbineId);
    }

    public async Task<bool> ChangeTurbineStatusAsync(int turbineId, TurbineStatus status)
    {
        return await _turbineRepository.ChangeTurbineStatusAsync(turbineId, status);
    }

    public async Task<List<TurbineDataReadDto>> GetTurbineDataHistoricalAsync(int turbineId, DateTime start, DateTime end)
    {
        var turbineData = await _turbineRepository.GetTurbineDataHistoricalAsync(turbineId, start, end);
        return _mapper.Map<List<TurbineDataReadDto>>(turbineData);
    }

    public async Task<TurbineDataReadDto> GetMostRecentTurbineDataAsync(int turbineId)
    {
        var turbineData = await _turbineRepository.GetMostRecentTurbineDataAsync(turbineId);
        return _mapper.Map<TurbineDataReadDto>(turbineData);
    }
}
