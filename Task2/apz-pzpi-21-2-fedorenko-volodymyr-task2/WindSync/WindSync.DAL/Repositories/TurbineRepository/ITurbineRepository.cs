using WindSync.Core.Enums;
using WindSync.Core.Models;

namespace WindSync.DAL.Repositories.TurbineRepository;

public interface ITurbineRepository
{
    Task<List<Turbine>> GetTurbinesAsync();
    Task<List<Turbine>> GetTurbinesByUserAsync(string usedId);
    Task<Turbine> GetTurbineByIdAsync(int turbineId);
    Task<int> AddTurbineAsync(Turbine turbine);
    Task<bool> UpdateTurbineAsync(Turbine turbine);
    Task<bool> DeleteTurbineAsync(int turbineId);
    Task<bool> ChangeTurbineStatusAsync(int turbineId, TurbineStatus status);
    Task<List<TurbineData>> GetTurbineDataHistoricalAsync(int turbineId, DateTime start, DateTime end);
    Task<TurbineData> GetMostRecentTurbineDataAsync(int turbineId);
}
