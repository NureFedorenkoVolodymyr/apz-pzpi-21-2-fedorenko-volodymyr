using WindSync.BLL.Dtos;

namespace WindSync.BLL.Services.AlertService;

public interface IAlertService
{
    Task<List<AlertDto>> GetAlertsByUserAsync(string userId);
    Task<bool> AddAlertAsync(AlertDto alert);
    Task<bool> DeleteAlertAsync(int alertId);
}
