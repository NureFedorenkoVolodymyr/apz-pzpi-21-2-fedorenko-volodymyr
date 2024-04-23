using WindSync.Core.Enums;

namespace WindSync.PL.ViewModels;

public class TurbineViewModel
{
    public int Id { get; set; }
    public TurbineStatus Status { get; set; }
    public double SweptArea { get; set; }
    public int WindFarmId { get; set; }
}
