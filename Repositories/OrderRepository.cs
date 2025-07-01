
using DotShop.API.Data;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;
    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        var newOrder = await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return newOrder.Entity;

    }
    public async Task<List<Order>> GetOrdersByUserId(Guid userId)
    {
        return await _dbContext.Orders
       .Include(o => o.Items)
       .Where(o => o.UserId == userId)
       .ToListAsync();
    }
    public async Task<Order?> FindById(Guid id)
    {
        return await _dbContext.Orders.FindAsync(id);
    }
    public async Task<Order?> Delete(Guid id)

    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null) return null;
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }
    public async Task<Order?> FindByIdAndUserId(Guid orderId, Guid userId)
    {
        return await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
    }
    public async Task<Order?> Update(Order order)
    {
        var existingOrder = await _dbContext.Orders.FindAsync(order.Id);
        if (existingOrder == null) return null;
        existingOrder.Items = order.Items;
        existingOrder.TotalPrice = order.TotalPrice;

        //
        _dbContext.Orders.Update(existingOrder);
        await _dbContext.SaveChangesAsync();
        return existingOrder;
    }
}

