using WindSync.BLL.Dtos;

namespace WindSync.BLL.Services;

public interface ITurbineDataService
{
    Task<bool> AddDataAsync(TurbineDataDto data);
    Task<bool> DeleteDataAsync(int dataId);
}
