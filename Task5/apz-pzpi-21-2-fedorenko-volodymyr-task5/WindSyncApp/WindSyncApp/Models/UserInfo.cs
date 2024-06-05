namespace WindSyncApp.Models;

public class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
