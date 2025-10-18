namespace DotShop.API.Controllers;

using AutoMapper;
using DotShop.API.Interfaces;
using DotShop.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.WSTrust;

[Route("api/user")]
[ApiController]
public class AuthController : Controller
{
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMapper mappper, IAuthService authService, ILogger<AuthController> logger)
    {
        _mapper = mappper;
        _authService = authService;
        _logger = logger;
    }


    [HttpPost("register")]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDto)
    {

        var result = await _authService.Register(registerDto.Username, registerDto.Password, registerDto.Roles);

        if (!result.Success)
            return BadRequest(new
            {
                status = "fail",
                message = result.Message
            });

        return Created("", new
        {
            status = "success",
            message = result.Message
        });

    }
    [HttpPost("login")]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var result = await _authService.Login(loginRequest.Username, loginRequest.Password);
        if (result.Success == false)
            return Unauthorized(new
            {
                Status = "fail",
                result.Message
            });

        return Ok(new
        {
            status = "success",
            token = result.Token,
            tokenType = "Bearer",

        });

    }
    [HttpPatch("changePassword")]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePassword)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            _logger.LogWarning("Unauthorized user tried to change password.");
            return Unauthorized(new { status = "fail", message = "Unauthorized user." });
        }

        var result = await _authService.ChangePassword(username, changePassword.OldPassword, changePassword.NewPassword);
        if (!result.Success) return BadRequest(new
        {
            status = result.Status,
            message = result.Message
        });

        return Ok(new
        {
            status = result.Status,
            message = result.Message
        });

    }
    [HttpPost("forgotPassword")]
    [ValidateModel]

    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto forgotPassword, CancellationToken ct)
    {
        var result = await _authService.ForgotPassword(forgotPassword.Username, ct);
        _logger.LogInformation("Password reset requested for user (masked): {User}", MaskUsername(forgotPassword.Username));
        return Accepted(new { status = "success", message = "If an account with that username exists, a reset link has been sent." });
    }
    [HttpPost("resetPassword")]
    [ValidateModel]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPassword)
    {

        var result = await _authService.ResetPassword(resetPassword.UserName, resetPassword.Token, resetPassword.NewPassword);
        if (!result.Success)
            return BadRequest(new { status = "fail", message = result.Message });


        return Ok(new { status = "success", message = result.Message });

    }
    [HttpDelete("deleteAccount")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new { status = "fail", message = "Unauthorized user." });
        }
        var res = await _authService.DeleteAccount(username);

        if (!res.Success)
            return BadRequest(new { status = res.Status, message = res.Message });

        return Ok(new { status = res.Status, message = res.Message });
    }
    private static string MaskUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return username;
        if (username.Length <= 2) return "***";
        return username.Substring(0, 1) + new string('*', Math.Max(0, username.Length - 2)) + username.Substring(username.Length - 1);
    }
}