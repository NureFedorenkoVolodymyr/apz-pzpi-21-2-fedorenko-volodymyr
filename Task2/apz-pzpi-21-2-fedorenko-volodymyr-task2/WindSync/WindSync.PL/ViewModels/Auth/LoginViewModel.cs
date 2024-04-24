using System.ComponentModel.DataAnnotations;

namespace WindSync.PL.ViewModels.Auth;

public class LoginViewModel
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
