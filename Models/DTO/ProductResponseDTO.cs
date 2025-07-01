using System.ComponentModel.DataAnnotations;

namespace DotShop.API.Models.DTO;

public class ProductResponseDTO
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    public string ImageUrl { get; set; }

    [Required]
    public int Stock { get; set; }
}