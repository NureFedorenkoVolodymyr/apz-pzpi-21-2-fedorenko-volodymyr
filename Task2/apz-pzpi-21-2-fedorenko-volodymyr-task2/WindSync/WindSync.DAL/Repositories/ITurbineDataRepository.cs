using WindSync.Core.Models;

namespace WindSync.DAL.Repositories;

public interface ITurbineDataRepository
{
    Task<bool> AddDataAsync(TurbineData data);
    Task<bool> DeleteDataAsync(int dataId);
}
