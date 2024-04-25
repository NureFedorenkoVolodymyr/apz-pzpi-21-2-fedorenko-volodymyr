using WindSync.Core.Models;

namespace WindSync.DAL.Repositories.TurbineDataRepository;

public interface ITurbineDataRepository
{
    Task<bool> AddDataAsync(TurbineData data);
    Task<bool> DeleteDataAsync(int dataId);
}
