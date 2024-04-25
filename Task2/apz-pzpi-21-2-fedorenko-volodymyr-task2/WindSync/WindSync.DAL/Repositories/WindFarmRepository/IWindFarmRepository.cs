using WindSync.Core.Models;

namespace WindSync.DAL.Repositories.WindFarmRepository;

public interface IWindFarmRepository
{
    Task<List<WindFarm>> GetFarmsAsync();
    Task<List<WindFarm>> GetFarmsByUserAsync(string userId);
    Task<WindFarm> GetFarmByIdAsync(int farmId);
    Task<int> AddFarmAsync(WindFarm farm);
    Task<bool> UpdateFarmAsync(WindFarm farm);
    Task<bool> DeleteFarmAsync(int farmId);
    Task<List<Turbine>> GetTurbinesByFarmAsync(int farmId);
}
