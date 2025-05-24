

using DotShop.API.Data;
using DotShop.API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext dbContext)
    {
        this._context = dbContext;

    }

    public async Task<List<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException("Product not found");
        }
    }
    public async Task<Product> Update(Guid id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingProduct;
        }
        else
        {
            throw new KeyNotFoundException("Product not found");
        }

    }
}