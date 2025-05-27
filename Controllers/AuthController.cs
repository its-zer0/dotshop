namespace DotShop.API.Controllers;

using AutoMapper;
using DotShop.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/")]
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

        if (result == null)
        {
            return BadRequest("Registration failed.");
        }

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
                Message = result.Message
            });

        return Ok(new
        {
            Status = "success",
            Message = result.Message,
            Token = result.Token
        });

    }
}