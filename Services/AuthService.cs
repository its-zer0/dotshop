

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
    public async Task<AuthResult> Register(string username, string password, string[] roles)
    {
        var identityUser = new IdentityUser
        {
            UserName = username,
            Email = username
        };
        var res = await _userManager.CreateAsync(identityUser, password);
        if (res.Succeeded)
        {
            if (roles != null && roles.Length > 0)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) return new AuthResult
                {
                    Success = false,
                    Message = "User creation failed unexpectedly."
                };

                res = await _userManager.AddToRolesAsync(user, roles);
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

    public async Task<AuthResult> Login(string username, string password)
    {
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
}