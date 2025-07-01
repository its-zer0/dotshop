using Microsoft.AspNetCore.Identity;
public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public IdentityUser User { get; set; }
    public ICollection<OrderItem> Items { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }
}