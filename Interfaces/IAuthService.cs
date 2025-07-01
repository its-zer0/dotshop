
namespace DotShop.API.Interfaces;

public interface IAuthService
{
    Task<AuthResult> Register(string username, string password, string[] roles);
    Task<AuthResult> Login(string username, string password);
    Task<AuthResult> ChangePassword(string username, string oldPassword, string newPassword);
    Task<AuthResult> ForgotPassword(string username);
    Task<AuthResult> ResetPassword(string username, string token, string newPassword);
    Task<AuthResult> DeleteAccount(string username);
}