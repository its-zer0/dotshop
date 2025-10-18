
using DotShop.API.Models.Domain;
using DotShop.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
namespace DotShop.API.Interfaces;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllProducts();
    // Task<IActionResult> GetProductById(int id);
    Task<ProductDTO> CreateProduct(CreateProductDTO createProduct);
    Task DeleteProduct(Guid id);
    Task<ProductDTO> GetById(Guid id);
    Task<ProductDTO> UpdateProduct(Guid id, ProductDTO updateProduct);

}