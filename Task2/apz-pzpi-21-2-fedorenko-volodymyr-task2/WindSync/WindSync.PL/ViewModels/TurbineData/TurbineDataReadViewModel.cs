namespace WindSync.PL.ViewModels.TurbineData;

public class TurbineDataReadViewModel
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public double WindSpeed { get; set; }
    public double AirTemperature { get; set; }
    public double AirPressure { get; set; }
    public double AirDensity { get; set; }
    public double RatedPower { get; set; }
    public double PowerOutput { get; set; }
    public int TurbineId { get; set; }
}
