namespace WindSync.Core.Models;

public class TurbineData
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public double WindSpeed { get; set; }
    public double Temperature { get; set; }
    public double PowerOutput {  get; set; }
    public int TurbineId { get; set; }
    public Turbine? Turbine { get; set; }
}
