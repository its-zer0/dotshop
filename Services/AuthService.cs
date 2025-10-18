

using DotShop.API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.WSTrust;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAuthRepository _authRepository;

    public AuthService(UserManager<IdentityUser> userManager, IAuthRepository authRepository)
    {
        _userManager = userManager;
        _authRepository = authRepository;
    }
    public async Task<AuthResult> Register(string username, string password, List<string> roles, CancellationToken ct)
    {

        ct.ThrowIfCancellationRequested();
        var identityUser = new IdentityUser
        {
            UserName = username,
            Email = username
        };
        var res = await _userManager.CreateAsync(identityUser, password);
        if (res.Succeeded)
        {
            if (roles != null && roles.Count > 0)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) return new AuthResult
                {
                    Success = false,
                    Message = "User creation failed unexpectedly."
                };

                res = await _userManager.AddToRolesAsync(user, roles.ToArray());
                if (!res.Succeeded) return new AuthResult
                {
                    Success = false,
                    Message = "Failed to assign roles to user."
                };
                return new AuthResult
                {
                    Success = true,
                    Message = $"User {username} registered successfully with roles: {string.Join(", ", roles)}."
                };
            }
            return new AuthResult
            {
                Success = true,
                Message = $"User {username} registered successfully without roles."
            };
        }
        else return new AuthResult
        {
            Success = false,
            Message = $"User registration failed: {string.Join(", ", res.Errors.Select(e => e.Description))}."
        };
    }

    public async Task<AuthResult> Login(string username, string password, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var user = await _userManager.FindByEmailAsync(username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {

            return new AuthResult
            {
                Success = false,
                Message = "Incorrect username or password."
            };
        }
        var roles = await _userManager.GetRolesAsync(user);

        //  Generate a JWT
        var token = _authRepository.CreateJwtToken(user, roles.ToList());
        return new AuthResult
        {
            Success = true,
            Message = "Login successful.",
            Token = token
        };


    }

    public async Task<AuthResult> ChangePassword(string username, string oldPassword, string newPassword, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return new AuthResult
            {
                Success = false,
                Message = "User not found."
            };

        var res = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!res.Succeeded)
            return new AuthResult
            {
                Success = false,
                Message = string.Join("; ", res.Errors.Select(e => e.Description))
            };

        return new AuthResult
        {
            Success = true,
            Message = "Password changed successfully."
        };

    }


    public async Task<AuthResult> ForgotPassword(string username, CancellationToken ct)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return new AuthResult { Success = false, Message = "User not found." };

        ct.ThrowIfCancellationRequested();
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return new AuthResult
        {
            Success = true,
            Message = "Password reset token generated.",
            Token = token ?? "TOken"
        };
    }
    public async Task<AuthResult> ResetPassword(string username, string token, string newPassword, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return new AuthResult { Success = true, Message = "If the user exists, a password reset will be processed." };


        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            return new AuthResult
            {
                Success = false,
                Message = string.Join("; ", result.Errors.Select(e => e.Description))
            };
        return new AuthResult { Success = true, Message = "Password reset successful." };
    }
    public async Task<AuthResult> DeleteAccount(string username, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return new AuthResult { Success = false, Message = "User not found." };
        var res = await _userManager.DeleteAsync(user);
        if (!res.Succeeded) return new AuthResult
        {
            Success = false,
            Message = string.Join("; ", res.Errors.Select(e => e.Description))
        };
        return new AuthResult
        {
            Success = true,
            Message = "Account account deleted successfully."
        };
    }


}