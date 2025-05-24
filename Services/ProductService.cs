using AutoMapper;
using DotShop.API.Interfaces;
using DotShop.API.Models.Domain;
using DotShop.API.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DotShop.API.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repo;
    public ProductService(IMapper mapper, IProductRepository repo)
    {
        _mapper = mapper;
        _repo = repo;
    }
    public async Task<List<Product>> GetAllProducts()
    {
        // Simulate fetching products from a database
        var products = await _repo.GetAll();
        // Map the first product in the list to DTO

        return products;
    }
    public async Task<Product> CreateProduct(AddProductRequestDTO addProduct)
    {

        // 1) Map DTO to Domain Model
        var productDomain = _mapper.Map<Product>(addProduct);
        productDomain.CreatedAt = DateTime.UtcNow;
        productDomain.UpdatedAt = DateTime.UtcNow;

        // 2) Call repository to save the product
        await _repo.Create(productDomain);
        return productDomain;
    }
    public async Task DeleteProduct(Guid id)
    {

        await _repo.Delete(id);
    }
    public async Task<ProductResponseDTO> UpdateProduct(Guid id, UpdateProductRequestDTO updateProduct)
    {
        // 1) Map DTO to Domain Model
        var productDomain = _mapper.Map<Product>(updateProduct);
        productDomain.UpdatedAt = DateTime.UtcNow;

        // 2) Call repository to update the product
        var updatedProduct = await _repo.Update(id, productDomain);

        if (updatedProduct == null)
        {
            throw new KeyNotFoundException("Product not found");
        }

        return _mapper.Map<ProductResponseDTO>(updatedProduct);

    }

}