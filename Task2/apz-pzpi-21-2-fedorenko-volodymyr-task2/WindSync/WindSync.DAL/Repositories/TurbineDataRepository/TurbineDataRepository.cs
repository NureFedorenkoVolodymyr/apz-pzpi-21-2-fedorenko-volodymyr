using WindSync.Core.Models;
using WindSync.DAL.DB;

namespace WindSync.DAL.Repositories.TurbineDataRepository;

public class TurbineDataRepository : ITurbineDataRepository
{
    private readonly AppDbContext _dbContext;

    public TurbineDataRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddDataAsync(TurbineData data)
    {
        _dbContext.TurbineData.Add(data);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteDataAsync(int dataId)
    {
        var data = await _dbContext.TurbineData.FindAsync(dataId);
        if (data is null)
            return false;

        _dbContext.TurbineData.Remove(data);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
