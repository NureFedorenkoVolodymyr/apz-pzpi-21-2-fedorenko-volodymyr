using WindSyncApp.Models;

namespace WindSyncApp.Services;

public interface IApiService
{
    Task<bool> LoginAsync(string email, string password);
    bool IsAuthenticated();
    Task<List<Turbine>> GetMyTurbinesAsync();
    Task<List<TurbineData>> GetTurbineDataAsync(int turbineId, DateTime start, DateTime end);
}