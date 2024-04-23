using WindSync.Core.Enums;

namespace WindSync.Core.Models;

public class Turbine
{
    public int Id { get; set; }
    public TurbineStatus Status { get; set; }
    public double RatedPower { get; set; }
    public int WindFarmId { get; set; }
    public WindFarm? WindFarm { get; set; }
    public ICollection<TurbineData> TurbineData { get; set; } = new List<TurbineData>();
}
