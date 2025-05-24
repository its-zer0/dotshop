
using DotShop.API.Models.Domain;
using DotShop.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
namespace DotShop.API.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();
    // Task<IActionResult> GetProductById(int id);
    Task<Product> CreateProduct(AddProductRequestDTO addProduct);
    Task DeleteProduct(Guid id);
    Task<ProductResponseDTO> UpdateProduct(Guid id, UpdateProductRequestDTO updateProduct);

}