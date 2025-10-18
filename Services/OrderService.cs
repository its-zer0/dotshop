using AutoMapper;
using DotShop.API.Data;
using DotShop.Exceptions;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    private async Task<decimal> CalculateTotalPrice(IEnumerable<OrderItemDTO> items)
    {
        decimal total = 0m;

        foreach (var item in items)
        {
            var product = await _productRepository.GetById(item.ProductId);
            if (product == null)
                throw new ProductNotFoundException(item.ProductId);

            total += product.Price * item.Quantity;
        }

        return total;
    }

    public async Task<List<OrderResponseDTO>> GetAllOrders(Guid userId)
    {
        var orders = await _orderRepository.GetOrdersByUserId(userId);
        return _mapper.Map<List<OrderResponseDTO>>(orders);
    }

    public async Task<OrderResponseDTO> CreateOrder(AddOrderRequestDTO dto, Guid userId)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new BusinessException("Order must contain at least one item.");

        var order = _mapper.Map<Order>(dto);
        order.UserId = userId;
        order.TotalPrice = await CalculateTotalPrice(dto.Items);
        order.CreatedAt = DateTime.UtcNow;

        var createdOrder = await _orderRepository.CreateAsync(order);
        return _mapper.Map<OrderResponseDTO>(createdOrder);
    }

    public async Task<OrderResponseDTO> UpdateOrder(Guid orderId, UpdateOrderRequestDTO dto, Guid userId)
    {
        var order = await _orderRepository.FindByIdAndUserId(orderId, userId)
                    ?? throw new OrderNotFoundException(orderId);

        order.Items = _mapper.Map<List<OrderItem>>(dto.Items);
        order.TotalPrice = await CalculateTotalPrice(dto.Items);
        order.Status = dto.Status;

        await _orderRepository.Update(order);
        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task DeleteOrder(Guid orderId, Guid userId)
    {
        var order = await _orderRepository.FindByIdAndUserId(orderId, userId)
                    ?? throw new OrderNotFoundException(orderId);

        await _orderRepository.Delete(order.Id);
    }
}
