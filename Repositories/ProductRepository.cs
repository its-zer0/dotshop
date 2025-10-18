using DotShop.API.Data;
using DotShop.API.Models.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DotShop.Exceptions;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(AppDbContext dbContext, ILogger<ProductRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<Product>> GetAll()
    {
        try
        {
            return await _dbContext.Products.ToListAsync();
        }
        catch (DbUpdateException dbex)
        {
            _logger.LogError(dbex, "EF/DB update error while fetching all products.");
            throw new DataAccessException("Failed to fetch products.", dbex);
        }
    }

    public async Task Create(Product product)
    {
        try
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        catch (DbUpdateException dbex)
        {
            _logger.LogError(dbex, "EF/DB update error while creating product ({ProductName}).", product?.Name);
            throw new DataAccessException("Failed to create product.", dbex);
        }
    }

    public async Task Delete(Guid id)
    {
        try
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                throw new ProductNotFoundException(id);

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
        catch (ProductNotFoundException) // let domain exception flow up
        {
            throw;
        }

        catch (DbUpdateException dbex)
        {
            _logger.LogError(dbex, "EF/DB update error while deleting product {ProductId}.", id);
            throw new DataAccessException("Failed to delete product.", dbex);
        }
    }

    public async Task<Product> Update(Guid id, Product product)
    {
        try
        {
            var existingProduct = await _dbContext.Products.FindAsync(id);
            if (existingProduct == null)
                throw new ProductNotFoundException(id);

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }
        catch (ProductNotFoundException) // bubble up
        {
            throw;
        }

        catch (DbUpdateException dbex)
        {
            _logger.LogError(dbex, "EF/DB update error while updating product {ProductId}.", id);
            throw new DataAccessException("Failed to update product.", dbex);
        }
    }

    public async Task<Product?> GetById(Guid id)
    {
        try
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                throw new ProductNotFoundException(id);
            return product;
        }

        catch (DbUpdateException dbex)
        {
            _logger.LogError(dbex, "EF/DB update error while fetching product by id {ProductId}.", id);
            throw new DataAccessException("Failed to fetch product by ID.", dbex);
        }
    }
}
