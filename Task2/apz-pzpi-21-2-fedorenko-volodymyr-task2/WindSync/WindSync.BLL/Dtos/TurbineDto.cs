using WindSync.Core.Enums;
using WindSync.Core.Models;

namespace WindSync.BLL.Dtos;

public class TurbineDto
{
    public int Id { get; set; }
    public TurbineStatus Status { get; set; }
    public double RatedPower { get; set; }
    public int WindFarmId { get; set; }
}
