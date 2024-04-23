using WindSync.BLL.Dtos;

namespace WindSync.BLL.Services;

public interface IWindFarmService
{
    Task<List<WindFarmDto>> GetFarmsByUserAsync(string userId);
    Task<WindFarmDto> GetFarmByIdAsync(int farmId);
    Task<bool> AddFarmAsync(WindFarmDto farm);
    Task<bool> UpdateFarmAsync(WindFarmDto farm);
    Task<bool> DeleteFarmAsync(int farmId);
    Task<List<TurbineDto>> GetTurbinesByFarmAsync(int farmId);
}
