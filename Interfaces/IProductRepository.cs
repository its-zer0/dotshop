
using DotShop.API.Models.Domain;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task Create(Product product);
    Task Delete(Guid id);
    Task<Product> Update(Guid id, Product product);
    Task<Product?> GetById(Guid id);
}
