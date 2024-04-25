using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.DAL.Repositories.WindFarmRepository;

namespace WindSync.BLL.Services.WindFarmService;

public class WindFarmService : IWindFarmService
{
    private readonly IWindFarmRepository _windFarmRepository;
    private readonly IMapper _mapper;

    public WindFarmService(IWindFarmRepository windFarmRepository, IMapper mapper)
    {
        _windFarmRepository = windFarmRepository;
        _mapper = mapper;
    }

    public async Task<List<WindFarmDto>> GetFarmsAsync()
    {
        var farms = await _windFarmRepository.GetFarmsAsync();
        return _mapper.Map<List<WindFarmDto>>(farms);
    }

    public async Task<List<WindFarmDto>> GetFarmsByUserAsync(string userId)
    {
        var farms = await _windFarmRepository.GetFarmsByUserAsync(userId);
        return _mapper.Map<List<WindFarmDto>>(farms);
    }

    public async Task<WindFarmDto> GetFarmByIdAsync(int farmId)
    {
        var farm = await _windFarmRepository.GetFarmByIdAsync(farmId);
        return _mapper.Map<WindFarmDto>(farm);
    }

    public async Task<int> AddFarmAsync(WindFarmDto farmDto)
    {
        var farm = _mapper.Map<WindFarm>(farmDto);
        return await _windFarmRepository.AddFarmAsync(farm);
    }

    public async Task<bool> UpdateFarmAsync(WindFarmDto farmDto)
    {
        var farm = _mapper.Map<WindFarm>(farmDto);
        return await _windFarmRepository.UpdateFarmAsync(farm);
    }

    public async Task<bool> DeleteFarmAsync(int farmId)
    {
        return await _windFarmRepository.DeleteFarmAsync(farmId);
    }

    public async Task<List<TurbineDto>> GetTurbinesByFarmAsync(int farmId)
    {
        var turbines = await _windFarmRepository.GetTurbinesByFarmAsync(farmId);
        return _mapper.Map<List<TurbineDto>>(turbines);
    }
}
