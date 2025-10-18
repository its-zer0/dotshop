using AutoMapper;
using DotShop.API.Interfaces;
using DotShop.API.Models.Domain;
using DotShop.API.Models.DTO;
using DotShop.Exceptions;
using Microsoft.Extensions.Logging;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repo;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IMapper mapper, IProductRepository repo, ILogger<ProductService> logger)
    {
        _mapper = mapper;
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<ProductDTO>> GetAllProducts()
    {
        var products = await _repo.GetAll();
        return _mapper.Map<List<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid product ID.", nameof(id));

        var product = await _repo.GetById(id); // may throw ProductNotFoundException
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> CreateProduct(CreateProductDTO create)
    {
        // business validation example
        if (create.Price < 0) throw new BusinessException("Price cannot be negative.");

        var product = _mapper.Map<Product>(create);
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        await _repo.Create(product);
        _logger.LogInformation("Created product {ProductId} ({ProductName})", product.Id, product.Name);

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task DeleteProduct(Guid id)
    {
        await _repo.Delete(id); // may throw ProductNotFoundException
        _logger.LogInformation("Deleted product {ProductId}", id);
    }

    public async Task<ProductDTO> UpdateProduct(Guid id, ProductDTO dto)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid product ID.", nameof(id));

        var product = _mapper.Map<Product>(dto);
        product.Id = id;
        product.UpdatedAt = DateTime.UtcNow;

        var updated = await _repo.Update(id, product); // may throw ProductNotFoundException
        _logger.LogInformation("Updated product {ProductId}", id);

        return _mapper.Map<ProductDTO>(updated);
    }
}
