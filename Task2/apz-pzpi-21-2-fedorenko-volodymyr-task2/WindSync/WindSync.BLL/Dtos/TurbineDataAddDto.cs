namespace WindSync.BLL.Dtos;

public class TurbineDataAddDto
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public double WindSpeed { get; set; }
    public double AirTemperature { get; set; }
    public double AirPressure { get; set; }
    public int TurbineId { get; set; }
}
