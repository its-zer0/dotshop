
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class AuthRepository : IAuthRepository
{
    private IConfiguration _config;

    public AuthRepository(IConfiguration config)
    {
        _config = config;

    }
    public string CreateJwtToken(IdentityUser user, List<string> roles)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_config["JWT:Issuer"], _config["JWT:Audience"], claims, expires: DateTime.Now.AddDays(9), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}