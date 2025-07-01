using System.ComponentModel.DataAnnotations;

public class OrderResponseDTO
{
    public Guid Id { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }

    [Required]
    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    public List<OrderItemResponseDTO> Items { get; set; }
}

public class OrderItemResponseDTO
{
    public Guid ProductId { get; set; }


    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PriceAtPurchase { get; set; }
}