using WindSync.Core.Enums;

namespace WindSync.BLL.Dtos;

public class TurbineDto
{
    public int Id { get; set; }
    public TurbineStatus Status { get; set; }
    public double TurbineRadius { get; set; }
    public double SweptArea { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public int Efficiency { get; set; }
    public int CutInWindSpeed { get; set; }
    public int RatedWindSpeed { get; set; }
    public int ShutDownWindSpeed { get; set; }
    public int WindFarmId { get; set; }
}
