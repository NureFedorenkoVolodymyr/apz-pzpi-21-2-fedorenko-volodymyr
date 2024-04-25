using Microsoft.EntityFrameworkCore;
using WindSync.Core.Models;
using WindSync.DAL.DB;

namespace WindSync.DAL.Repositories.AlertRepository;

public class AlertRepository : IAlertRepository
{
    private readonly AppDbContext _dbContext;

    public AlertRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Alert>> GetAlertsByUserAsync(string userId)
    {
        return await _dbContext.Alerts
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> AddAlertAsync(Alert alert)
    {
        _dbContext.Alerts.Add(alert);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAlertAsync(int alertId)
    {
        var alert = await _dbContext.Alerts.FindAsync(alertId);
        if (alert is null)
            return false;

        _dbContext.Alerts.Remove(alert);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
