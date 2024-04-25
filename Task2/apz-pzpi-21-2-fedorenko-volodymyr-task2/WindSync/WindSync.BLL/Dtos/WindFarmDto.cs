namespace WindSync.BLL.Dtos;

public class WindFarmDto
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public required string UserId { get; set; }
}
