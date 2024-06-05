namespace WindSyncApp.Models;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public ICollection<WindFarm> WindFarms { get; set; } = new List<WindFarm>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
