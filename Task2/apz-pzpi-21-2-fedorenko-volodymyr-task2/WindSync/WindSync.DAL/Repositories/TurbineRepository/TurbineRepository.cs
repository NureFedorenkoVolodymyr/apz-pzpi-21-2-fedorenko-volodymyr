using Microsoft.EntityFrameworkCore;
using WindSync.Core.Enums;
using WindSync.Core.Models;
using WindSync.DAL.DB;

namespace WindSync.DAL.Repositories.TurbineRepository;

public class TurbineRepository : ITurbineRepository
{
    private readonly AppDbContext _dbContext;

    public TurbineRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Turbine>> GetTurbinesAsync()
    {
        return await _dbContext.Turbines
            .Include(t => t.WindFarm)
            .ToListAsync();
    }

    public async Task<List<Turbine>> GetTurbinesByUserAsync(string userId)
    {
        return await _dbContext.Turbines
            .Include(t => t.WindFarm)
            .Where(t => t.WindFarm.UserId == userId)
            .ToListAsync();
    }

    public async Task<Turbine> GetTurbineByIdAsync(int turbineId)
    {
        return await _dbContext.Turbines
            .Include(t => t.WindFarm)
            .FirstOrDefaultAsync(t => t.Id == turbineId);
    }

    public async Task<int> AddTurbineAsync(Turbine turbine)
    {
        _dbContext.Turbines.Add(turbine);
        await _dbContext.SaveChangesAsync();
        return turbine.Id;
    }

    public async Task<bool> UpdateTurbineAsync(Turbine turbine)
    {
        var dbTurbine = await _dbContext.Turbines.FindAsync(turbine.Id);
        if (dbTurbine is null)
            return false;

        dbTurbine.TurbineRadius = turbine.TurbineRadius;
        dbTurbine.SweptArea = turbine.SweptArea;
        dbTurbine.Latitude = turbine.Latitude;
        dbTurbine.Longitude = turbine.Longitude;
        dbTurbine.Altitude = turbine.Altitude;
        dbTurbine.Efficiency = turbine.Efficiency;
        dbTurbine.CutInWindSpeed = turbine.CutInWindSpeed;
        dbTurbine.RatedWindSpeed = turbine.RatedWindSpeed;
        dbTurbine.ShutDownWindSpeed = turbine.ShutDownWindSpeed;
        dbTurbine.WindFarmId = turbine.WindFarmId;

        _dbContext.Turbines.Update(dbTurbine);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteTurbineAsync(int turbineId)
    {
        var turbine = await _dbContext.Turbines.FindAsync(turbineId);
        if (turbine is null)
            return false;

        _dbContext.Turbines.Remove(turbine);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> ChangeTurbineStatusAsync(int turbineId, TurbineStatus status)
    {
        var turbine = await _dbContext.Turbines.FindAsync(turbineId);
        if (turbine is null)
            return false;

        turbine.Status = status;
        _dbContext.Turbines.Update(turbine);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<TurbineData>> GetTurbineDataHistoricalAsync(int turbineId, DateTime start, DateTime end)
    {
        return await _dbContext.TurbineData
            .Where(td => td.TurbineId == turbineId && td.DateTime >= start && td.DateTime <= end)
            .ToListAsync();
    }

    public async Task<TurbineData> GetMostRecentTurbineDataAsync(int turbineId)
    {
        return await _dbContext.TurbineData
            .Where(td => td.TurbineId == turbineId)
            .OrderByDescending(td => td.DateTime)
            .FirstOrDefaultAsync();
    }
}
