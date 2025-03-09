using System.ComponentModel.DataAnnotations;

namespace OdysseyWebServer.Models.ViewModels;

public class LoginViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
    public string? Username { get; set; } = string.Empty;
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    public string? Password { get; set; } = string.Empty;
}