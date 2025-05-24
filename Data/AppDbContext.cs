

using DotShop.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotShop.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
}