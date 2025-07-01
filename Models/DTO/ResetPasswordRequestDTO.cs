using System.ComponentModel.DataAnnotations;

namespace DotShop.API.Models.DTO;

public class ResetPasswordRequestDto
{
    [Required(ErrorMessage = "Please provide a username")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please provide token!")]
    public string Token { get; set; }

    [Required(ErrorMessage = "NewPassword is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
    public string NewPassword { get; set; }
}