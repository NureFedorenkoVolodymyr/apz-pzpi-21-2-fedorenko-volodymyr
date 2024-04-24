using WindSync.Core.Enums;
using WindSync.Core.Models;

namespace WindSync.DAL.Repositories.AlertRepository;

public interface IAlertRepository
{
    Task<List<Alert>> GetAlertsByUserAsync(string userId);
    Task<bool> AddAlertAsync(Alert alert);
    Task<bool> DeleteAlertAsync(int alertId);
}
