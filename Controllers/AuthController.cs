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

    public AuthController(IMapper mappper, IAuthService authService)
    {
        _mapper = mappper;
        _authService = authService;

    }


    [HttpPost("register")]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDto)
    {

        var result = await _authService.Register(registerDto.Username, registerDto.Password, registerDto.Roles);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok();

    }
    [HttpPost("login")]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var result = await _authService.Login(loginRequest.Username, loginRequest.Password);
        if (result.Success == false)
            return BadRequest(new
            {
                Status = "fail",
                result.Message
            });

        return Ok(new
        {
            Status = "success",
            result.Message,
            result.Token
        });

    }
    [HttpPatch("changePassword")]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePassword)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return Unauthorized(new
        {
            Status = "fail",
            Message = "Unauthorized user."
        });
        var result = await _authService.ChangePassword(username, changePassword.OldPassword, changePassword.NewPassword);
        if (result.Success == false)
        {
            return BadRequest(new
            {
                result.Status,
                result.Message
            });
        }
        return Ok(new
        {
            result.Status,
            result.Message
        });

    }
    [HttpPost("forgotPassword")]
    [ValidateModel]

    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto forgotPassword)
    {

        var result = await _authService.ForgotPassword(forgotPassword.Username);
        if (!result.Success) return BadRequest();
        return Ok(new
        {
            result.Status,
            result.Message,
            result.Token
        });
    }
    [HttpPost("resetPassword")]
    [ValidateModel]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPassword)
    {

        var result = await _authService.ResetPassword(resetPassword.UserName, resetPassword.Token, resetPassword.NewPassword);
        if (!result.Success) return BadRequest();

        return Ok(new
        {
            result.Status,
            result.Message
        });

    }
    [HttpDelete("deleteAccount")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return Unauthorized(new
        {
            Status = "fail",
            Message = "Unauthorized user."
        });
        var res = await _authService.DeleteAccount(username);

        if (!res.Success) return BadRequest(new
        {
            res.Status,
            res.Message
        });
        return Ok(new
        {
            res.Status,
            res.Message
        });
    }
}