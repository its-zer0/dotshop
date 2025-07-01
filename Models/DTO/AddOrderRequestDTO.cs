using System.ComponentModel.DataAnnotations;

public class AddOrderRequestDTO
{
    [Required(ErrorMessage = "Order must contain at least one item.")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<OrderItemDTO> Items { get; set; }
}

public class OrderItemDTO
{
    [Required(ErrorMessage = "Product ID is required.")]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}