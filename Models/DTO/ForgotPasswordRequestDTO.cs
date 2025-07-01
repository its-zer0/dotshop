using System.ComponentModel.DataAnnotations;

public class ForgotPasswordRequestDto
{
    [Required(ErrorMessage = "Username is required!")]
    [DataType(DataType.Text)]
    public string Username { get; set; }
}