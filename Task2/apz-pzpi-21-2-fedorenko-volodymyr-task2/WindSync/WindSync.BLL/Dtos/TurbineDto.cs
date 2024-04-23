using WindSync.Core.Enums;

namespace WindSync.BLL.Dtos;

public class TurbineDto
{
    public int Id { get; set; }
    public TurbineStatus Status { get; set; }
    public double SweptArea { get; set; }
    public int WindFarmId { get; set; }
}
