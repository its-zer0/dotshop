namespace DotShop.API.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AuthDbContext : IdentityDbContext<IdentityUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)

    {


    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var adminId = "b7e8a1c2-3f4d-4e5f-8a6b-1c2d3e4f5a6b";
        var userId = "c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f";
        var roles = new List<IdentityRole>
        {
            new IdentityRole {Id = adminId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole {Id = userId, Name = "User", NormalizedName = "USER" }
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}