using WindSync.Core.Enums;
using WindSync.Core.Models;

namespace WindSync.BLL.Dtos;

public class TurbineDataDto
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public double WindSpeed { get; set; }
    public double Temperature { get; set; }
    public double PowerOutput { get; set; }
    public int TurbineId { get; set; }
}
