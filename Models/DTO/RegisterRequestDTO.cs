
using System.ComponentModel.DataAnnotations;

public class RegisterRequestDTO
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username must be between 3 and 100 characters.", MinimumLength = 3)]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    public string[] Roles { get; set; } = new string[] { "User" }; // Default role is User

}