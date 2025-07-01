

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("/api/orders")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrders(User.GetUserId());
        return Ok(orders);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateOrder([FromBody] AddOrderRequestDTO addOrder)
    {
        if (addOrder == null)
        {
            return BadRequest("Order data is null.");
        }
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(userIdClaim!.Value);
        var createdOrder = await _orderService.CreateOrder(addOrder, userId);
        return CreatedAtAction(nameof(GetAllOrders), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpDelete("{id:guid}")]

    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid order ID.");
        }

        await _orderService.DeleteOrder(id, User.GetUserId());
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderRequestDTO updateOrder)
    {
        if (id == Guid.Empty || updateOrder == null)
        {
            return BadRequest("Invalid order ID or order data.");
        }

        var updatedOrder = await _orderService.UpdateOrder(id, updateOrder, User.GetUserId());
        return Ok(updatedOrder);
    }

}