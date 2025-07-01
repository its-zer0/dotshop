using System.ComponentModel.DataAnnotations;

namespace DotShop.API.Models.DTO;

public class AddProductRequestDTO
{
    [Required]
    [StringLength(100, ErrorMessage = "Name must be between 2 and 100 characters.", MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Description must be between 10 and 500 characters.", MinimumLength = 10)]
    public string Description { get; set; }

    [Required]
    [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
    public string ImageUrl { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater.")]
    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}