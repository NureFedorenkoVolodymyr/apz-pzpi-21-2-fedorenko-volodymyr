using WindSync.BLL.Dtos;
using WindSync.Core.Enums;
using WindSync.Core.Models;

namespace WindSync.BLL.Services.TurbineDataService;

public interface ITurbineDataHelper
{
    Task<TurbineStatus> UpdateTurbineStatusNewAsync(Turbine turbine, TurbineData currentData);
    Task<TurbineStatus> UpdateTurbineStatusAsync(Turbine turbine, TurbineData currentData, TurbineData recentData);
    bool IsBelowCutInWindSpeed(Turbine turbine, double windSpeed);
    bool IsCutInWindSpeed(Turbine turbine, double windSpeed);
    bool IsRatedWindSpeed(Turbine turbine, double windSpeed);
    bool IsShutDownWindSpeed(Turbine turbine, double windSpeed);
    Task SendAlert(string message, AlertStatus status, Turbine turbine);
}
