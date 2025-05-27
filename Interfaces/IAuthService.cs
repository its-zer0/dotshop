
namespace DotShop.API.Interfaces;

public interface IAuthService
{
    Task<AuthResult> Register(string username, string password, string[] roles);
    Task<AuthResult> Login(string username, string password);
}