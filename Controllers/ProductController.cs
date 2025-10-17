using System.Threading.Tasks;
using AutoMapper;
using DotShop.API.Interfaces;
using DotShop.API.Models.Domain;
using DotShop.API.Models.DTO;
using DotShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromBody] AddProductRequestDTO addProduct)
    {
        if (addProduct == null)
        {
            return BadRequest("Product data is null.");
        }
        // Map the DTO to the domain model
        var productDomain = _mapper.Map<Product>(addProduct);

        var createdProduct = await _productService.CreateProduct(productDomain);
        return Ok(createdProduct);
        // return CreatedAtAction(nameof(GetAllProducts), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid product ID.");
        }

        await _productService.DeleteProduct(id);
        return NoContent();
    }
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequestDTO updateProduct)
    {
        if (id == Guid.Empty || updateProduct == null)
        {
            return BadRequest("Invalid product ID or product data.");
        }
        // Map the DTO to the domain model
        var productDomain = _mapper.Map<Product>(updateProduct);


        await _productService.UpdateProduct(id, productDomain);
        return NoContent();
    }

}

