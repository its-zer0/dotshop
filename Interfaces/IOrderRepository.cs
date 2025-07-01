
public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<List<Order>> GetOrdersByUserId(Guid userId);
    Task<Order?> FindById(Guid id);
    Task<Order?> Delete(Guid id);
    Task<Order?> FindByIdAndUserId(Guid orderId, Guid userId);
    Task<Order?> Update(Order order);
}