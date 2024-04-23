using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindSync.Core.Models;

public class WindFarm
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public required string UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Turbine> Turbines { get; set; } = new List<Turbine>();
}
