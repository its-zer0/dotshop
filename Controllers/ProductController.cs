using System.Threading.Tasks;
using DotShop.API.Interfaces;
using DotShop.API.Models.DTO;
using DotShop.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }


    [HttpGet]

    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] AddProductRequestDTO addProduct)
    {
        if (addProduct == null)
        {
            return BadRequest("Product data is null.");
        }

        var createdProduct = await _productService.CreateProduct(addProduct);
        return Ok(createdProduct);
        // return CreatedAtAction(nameof(GetAllProducts), new { id = createdProduct.Id }, createdProduct);
    }
    [HttpDelete("{id:guid}")]
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
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequestDTO updateProduct)
    {
        if (id == Guid.Empty || updateProduct == null)
        {
            return BadRequest("Invalid product ID or product data.");
        }

        // Assuming you have an UpdateProduct method in your service
        await _productService.UpdateProduct(id, updateProduct);
        return NoContent();
    }

}

