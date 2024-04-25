using WindSync.BLL.Dtos;

namespace WindSync.BLL.Services.WindFarmService;

public interface IWindFarmService
{
    Task<List<WindFarmDto>> GetFarmsAsync();
    Task<List<WindFarmDto>> GetFarmsByUserAsync(string userId);
    Task<WindFarmDto> GetFarmByIdAsync(int farmId);
    Task<int> AddFarmAsync(WindFarmDto farm);
    Task<bool> UpdateFarmAsync(WindFarmDto farm);
    Task<bool> DeleteFarmAsync(int farmId);
    Task<List<TurbineDto>> GetTurbinesByFarmAsync(int farmId);
}
