
public interface IOrderService
{
    Task<OrderResponseDTO> CreateOrder(AddOrderRequestDTO addOrderRequest, Guid userId);
    Task<List<OrderResponseDTO>> GetAllOrders(Guid userId);
    Task DeleteOrder(Guid id, Guid userId);

    Task<OrderResponseDTO> UpdateOrder(Guid id, UpdateOrderRequestDTO updateOrderRequest, Guid userId);

}