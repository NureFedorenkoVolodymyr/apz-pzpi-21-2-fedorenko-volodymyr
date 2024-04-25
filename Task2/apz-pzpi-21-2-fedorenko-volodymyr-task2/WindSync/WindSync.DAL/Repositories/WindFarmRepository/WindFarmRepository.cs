using Microsoft.EntityFrameworkCore;
using WindSync.Core.Models;
using WindSync.DAL.DB;

namespace WindSync.DAL.Repositories.WindFarmRepository;

public class WindFarmRepository : IWindFarmRepository
{
    private readonly AppDbContext _dbContext;

    public WindFarmRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<WindFarm>> GetFarmsAsync()
    {
        return await _dbContext.WindFarms
            .ToListAsync();
    }

    public async Task<List<WindFarm>> GetFarmsByUserAsync(string userId)
    {
        return await _dbContext.WindFarms
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task<WindFarm> GetFarmByIdAsync(int farmId)
    {
        return await _dbContext.WindFarms
            .FirstOrDefaultAsync(f => f.Id == farmId);
    }

    public async Task<int> AddFarmAsync(WindFarm farm)
    {
        _dbContext.WindFarms.Add(farm);
        await _dbContext.SaveChangesAsync();
        return farm.Id;
    }

    public async Task<bool> UpdateFarmAsync(WindFarm farm)
    {
        _dbContext.WindFarms.Update(farm);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteFarmAsync(int farmId)
    {
        var farm = await _dbContext.WindFarms.FindAsync(farmId);
        if (farm is null)
            return false;

        _dbContext.WindFarms.Remove(farm);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<Turbine>> GetTurbinesByFarmAsync(int farmId)
    {
        return await _dbContext.Turbines
            .Where(t => t.WindFarmId == farmId)
            .ToListAsync();
    }
}
