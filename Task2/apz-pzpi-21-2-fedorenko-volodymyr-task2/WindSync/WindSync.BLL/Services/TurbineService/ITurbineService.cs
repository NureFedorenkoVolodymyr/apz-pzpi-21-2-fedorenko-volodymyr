using WindSync.BLL.Dtos;
using WindSync.Core.Enums;

namespace WindSync.BLL.Services.TurbineService;

public interface ITurbineService
{
    Task<List<TurbineDto>> GetTurbinesAsync();
    Task<List<TurbineDto>> GetTurbinesByUserAsync(string usedId);
    Task<TurbineDto> GetTurbineByIdAsync(int turbineId);
    Task<int> AddTurbineAsync(TurbineDto turbine);
    Task<bool> UpdateTurbineAsync(TurbineDto turbine);
    Task<bool> DeleteTurbineAsync(int turbineId);
    Task<bool> ChangeTurbineStatusAsync(int turbineId, TurbineStatus status);
    Task<List<TurbineDataDto>> GetTurbineDataHistoricalAsync(int turbineId, DateTime start, DateTime end);
    Task<TurbineDataDto> GetMostRecentTurbineDataAsync(int turbineId);
}
