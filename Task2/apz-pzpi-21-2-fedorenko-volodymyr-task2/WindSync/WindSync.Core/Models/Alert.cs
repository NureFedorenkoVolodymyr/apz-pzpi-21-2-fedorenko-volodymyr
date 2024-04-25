using WindSync.Core.Enums;

namespace WindSync.Core.Models;

public class Alert
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public required string Message { get; set; }
    public AlertStatus Status { get; set; }
    public required string UserId { get; set; }
    public User? User { get; set; }
}
