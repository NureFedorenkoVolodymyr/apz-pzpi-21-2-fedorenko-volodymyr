using System.ComponentModel.DataAnnotations;

namespace WindSync.PL.ViewModels.Auth;

public class RegisterViewModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(4, ErrorMessage = "Password length must be greater than 4")]
    public string? Password { get; set; }
}
