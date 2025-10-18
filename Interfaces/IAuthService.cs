public interface IAuthService
{
    Task<AuthResult> Register(string username, string password, List<string> roles, CancellationToken ct = default);
    Task<AuthResult> Login(string username, string password, CancellationToken ct = default);
    Task<AuthResult> ChangePassword(string username, string oldPassword, string newPassword, CancellationToken ct = default);
    Task<AuthResult> ForgotPassword(string username, CancellationToken ct = default);
    Task<AuthResult> ResetPassword(string username, string token, string newPassword, CancellationToken ct = default);
    Task<AuthResult> DeleteAccount(string username, CancellationToken ct = default);
}
