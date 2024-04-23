using WindSync.Core.Models;

namespace WindSync.DAL.Repositories;

public interface IWindFarmRepository
{
    Task<List<WindFarm>> GetFarmsByUserAsync(string userId);
    Task<WindFarm> GetFarmByIdAsync(int farmId);
    Task<bool> AddFarmAsync(WindFarm farm);
    Task<bool> UpdateFarmAsync(WindFarm farm);
    Task<bool> DeleteFarmAsync(int farmId);
    Task<List<Turbine>> GetTurbinesByFarmAsync(int farmId);
}
