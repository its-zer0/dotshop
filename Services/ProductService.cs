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

        return await _repo.GetAll();

    }
    public async Task<Product> CreateProduct(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        await _repo.Create(product);
        return product;
    }
    public async Task DeleteProduct(Guid id)
    {

        await _repo.Delete(id);
    }
    public async Task<Product> UpdateProduct(Guid id, Product product)
    {

        product.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _repo.Update(id, product);

        if (updatedProduct == null)
        {
            throw new KeyNotFoundException("Product not found");
        }

        return updatedProduct;

    }

}