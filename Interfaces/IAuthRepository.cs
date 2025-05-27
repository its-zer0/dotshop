using Microsoft.AspNetCore.Identity;

public interface IAuthRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}