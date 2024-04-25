using Microsoft.AspNetCore.Identity;

namespace WindSync.Core.Models;

public class User : IdentityUser
{
    public ICollection<WindFarm> WindFarms { get; set; } = new List<WindFarm>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
