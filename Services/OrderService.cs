

using AutoMapper;
using DotShop.API.Data;
using DotShop.Exceptions;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _logger = logger;

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
        try
        {
            // Fetch all orders from the repository
            var orders = await _orderRepository.GetOrdersByUserId(userId);

            // Map to response DTOs
            var response = _mapper.Map<List<OrderResponseDTO>>(orders);
            return response;
        }
        catch (ProductNotFoundException ex)
        {
            _logger.LogError(ex, "Product not found while calculating total price.");
            throw;
        }
        catch (DataAccessException dex)
        {
            _logger.LogError(dex, "Failed to create order for user {UserId}", userId);
            throw new BusinessException("Failed to create order. Please try again later.", dex);
        }
    }

    public async Task<OrderResponseDTO> CreateOrder(AddOrderRequestDTO addOrderRequestDTO, Guid userId)
    {
        if (addOrderRequestDTO.Items == null) return null;

        try
        {
            // Map DTO to domain model
            var order = _mapper.Map<Order>(addOrderRequestDTO);
            order.UserId = userId;
            order.TotalPrice = await CalculateTotalPrice(addOrderRequestDTO.Items);
            order.CreatedAt = DateTime.UtcNow;

            // Save order
            var createdOrder = await _orderRepository.CreateAsync(order);

            // Map to response DTO
            return _mapper.Map<OrderResponseDTO>(createdOrder);

        }
        catch (ProductNotFoundException)
        {
            throw;
        }
        catch (DataAccessException dex)
        {
            _logger.LogError(dex, "Failed to create order for user {UserId}", userId);
            throw new BusinessException("Failed to create order. Please try again later.", dex);
        }
    }
    public async Task DeleteOrder(Guid id, Guid userId)
    {

        try
        {
            var order = await _orderRepository.FindByIdAndUserId(id, userId);
            if (order == null)
            {
                throw new Exception("Order not found or you do not have permission to delete this order.");
            }
            await _orderRepository.Delete(id);
        }
        catch (DataAccessException dex)
        {
            _logger.LogError(dex, "Failed to delete order {OrderId} for user {UserId}", id, userId);
            throw new BusinessException("Failed to delete order. Please try again later.", dex);

        }
    }

    public async Task<OrderResponseDTO> UpdateOrder(Guid id, UpdateOrderRequestDTO updateOrderRequest, Guid userId)
    {
        try
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
            return _mapper.Map<OrderResponseDTO>(order);

        }
        catch (ProductNotFoundException)
        {
            throw;
        }

        catch (DataAccessException dex)
        {
            _logger.LogError(dex, "Failed to update order {OrderId} for user {UserId}", id, userId);
            throw new BusinessException("Failed to update order. Please try again later.", dex);
        }
    }
}