using WindSync.BLL.Dtos;
using WindSync.Core.Enums;

namespace WindSync.BLL.Services;

public interface ITurbineService
{
    Task<List<TurbineDto>> GetTurbinesByUserAsync(string usedId);
    Task<TurbineDto> GetTurbineByIdAsync(int turbineId);
    Task<bool> AddTurbineAsync(TurbineDto turbine);
    Task<bool> UpdateTurbineAsync(TurbineDto turbine);
    Task<bool> DeleteTurbineAsync(int turbineId);
    Task<bool> ChangeTurbineStatusAsync(int turbineId, TurbineStatus status);
    Task<List<TurbineDataReadDto>> GetTurbineDataHistoricalAsync(int turbineId, DateTime start, DateTime end);
    Task<TurbineDataReadDto> GetMostRecentTurbineDataAsync(int turbineId);
}
