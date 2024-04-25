using WindSync.Core.Enums;

namespace WindSync.BLL.Dtos;

public class AlertDto
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public required string Message { get; set; }
    public AlertStatus Status { get; set; }
    public required string UserId { get; set; }
}
