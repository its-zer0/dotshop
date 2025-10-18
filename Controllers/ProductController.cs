using System;
using System.Threading.Tasks;
using AutoMapper;
using DotShop.API.Interfaces;
using DotShop.API.Models.DTO;
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

    // GET api/product
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts(); // returns List<ProductDTO>
        return Ok(products);
    }

    // GET api/product/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid product id.");

        var product = await _productService.GetById(id); // return ProductDTO or throw ProductNotFoundException
        return Ok(product);
    }

    // POST api/product
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDTO), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO addProduct)
    {
        if (addProduct == null) return BadRequest("Product data is null.");

        // Service accepts DTO and returns ProductDTO
        var created = await _productService.CreateProduct(addProduct);

        // Return 201 with location header - assumes GetById exists
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // DELETE api/product/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid product ID.");

        await _productService.DeleteProduct(id); // may throw ProductNotFoundException -> 404 via middleware
        return NoContent();
    }

    // PATCH api/product/{id}
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequestDTO updateProduct)
    {
        if (id == Guid.Empty || updateProduct == null) return BadRequest("Invalid product ID or product data.");

        // Service returns mapped DTO
        var productDto = _mapper.Map<ProductDTO>(updateProduct);
        await _productService.UpdateProduct(id, productDto);

        return NoContent();
    }
}
