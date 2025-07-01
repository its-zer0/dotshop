

using AutoMapper;
using DotShop.API.Data;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _productRepository = productRepository;
    }
    private async Task<decimal> CalculateTotalPrice(IEnumerable<OrderItemDTO> items)
    {
        decimal total = 0;
        foreach (var item in items)
        {
            var product = await _productRepository.GetById(item.ProductId);
            if (product == null)
                throw new Exception($"Product with ID {item.ProductId} not found.");

            total += product.Price * item.Quantity;
        }
        return total;
    }
    public async Task<List<OrderResponseDTO>> GetAllOrders(Guid userId)
    {
        // Fetch all orders from the repository
        var orders = await _orderRepository.GetOrdersByUserId(userId);

        // Map to response DTOs
        var response = _mapper.Map<List<OrderResponseDTO>>(orders);
        return response;
    }
    public async Task<OrderResponseDTO> CreateOrder(AddOrderRequestDTO addOrderRequestDTO, Guid userId)
    {
        // Map DTO to domain model
        var order = _mapper.Map<Order>(addOrderRequestDTO);
        if (addOrderRequestDTO.Items == null) return null;

        order.UserId = userId;

        order.TotalPrice = await CalculateTotalPrice(addOrderRequestDTO.Items);
        order.CreatedAt = DateTime.UtcNow;

        // Save order
        var createdOrder = await _orderRepository.CreateAsync(order);

        // Map to response DTO
        var response = _mapper.Map<OrderResponseDTO>(createdOrder);
        return response;
    }
    public async Task DeleteOrder(Guid id, Guid userId)
    {
        var order = await _orderRepository.FindByIdAndUserId(id, userId);
        if (order == null)
        {
            throw new Exception("Order not found or you do not have permission to delete this order.");
        }
        await _orderRepository.Delete(id);
    }

    public async Task<OrderResponseDTO> UpdateOrder(Guid id, UpdateOrderRequestDTO updateOrderRequest, Guid userId)
    {
        var order = await _orderRepository.FindByIdAndUserId(id, userId);
        if (order == null)
        {
            throw new Exception("Order not found or you do not have permission to update this order.");
        }
        // Update order properties
        order.Items = _mapper.Map<List<OrderItem>>(updateOrderRequest.Items);
        order.TotalPrice = await CalculateTotalPrice(updateOrderRequest.Items);
        order.Status = updateOrderRequest.Status;


        // Save updated order
        await _orderRepository.Update(order);
        // Map to response DTO
        var response = _mapper.Map<OrderResponseDTO>(order);
        return response;
    }

}