using WindSync.BLL.Dtos;
using WindSync.Core.Enums;

namespace WindSync.BLL.Services.TurbineDataService;

public interface ITurbineDataService
{
    Task<TurbineStatus> AddDataAsync(TurbineDataDto data);
    Task<bool> DeleteDataAsync(int dataId);
}
