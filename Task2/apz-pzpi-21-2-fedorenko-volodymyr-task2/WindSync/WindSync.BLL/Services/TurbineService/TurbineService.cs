using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Enums;
using WindSync.Core.Models;
using WindSync.DAL.Repositories.TurbineRepository;

namespace WindSync.BLL.Services.TurbineService;

public class TurbineService : ITurbineService
{
    private readonly ITurbineRepository _turbineRepository;
    private readonly IMapper _mapper;

    public TurbineService(ITurbineRepository turbineRepository, IMapper mapper)
    {
        _turbineRepository = turbineRepository;
        _mapper = mapper;
    }

    public async Task<List<TurbineDto>> GetTurbinesAsync()
    {
        var turbines = await _turbineRepository.GetTurbinesAsync();
        return _mapper.Map<List<TurbineDto>>(turbines);
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

    public async Task<int> AddTurbineAsync(TurbineDto turbineDto)
    {
        var turbine = _mapper.Map<Turbine>(turbineDto);
        turbine.Status = TurbineStatus.Idle;
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

    public async Task<List<TurbineDataDto>> GetTurbineDataHistoricalAsync(int turbineId, DateTime start, DateTime end)
    {
        var turbineData = await _turbineRepository.GetTurbineDataHistoricalAsync(turbineId, start, end);
        return _mapper.Map<List<TurbineDataDto>>(turbineData);
    }

    public async Task<TurbineDataDto> GetMostRecentTurbineDataAsync(int turbineId)
    {
        var turbineData = await _turbineRepository.GetMostRecentTurbineDataAsync(turbineId);
        return _mapper.Map<TurbineDataDto>(turbineData);
    }
}
