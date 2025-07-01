using System.ComponentModel.DataAnnotations;

public class UpdateOrderRequestDTO
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Order must contain at least one item.")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();

    [Range(0, double.MaxValue, ErrorMessage = "TotalPrice must be 0 or greater.")]
    public decimal TotalPrice { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Status must be between 1 and 50 characters.", MinimumLength = 1)]
    public string Status { get; set; } = "Pending";
}