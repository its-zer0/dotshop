using DotShop.API.Data;
using DotShop.Exceptions;
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
        try
        {
            var newOrder = await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return newOrder.Entity;
        }
        catch (DbUpdateException ex)
        {
            throw new DataAccessException("Failed to create order.", ex);
        }
    }

    public async Task<List<Order>> GetOrdersByUserId(Guid userId)
    {
        try
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DataAccessException("Failed to fetch orders.", ex);
        }
    }

    public async Task<Order?> FindByIdAndUserId(Guid orderId, Guid userId)
    {
        try
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }
        catch (DbUpdateException ex)
        {
            throw new DataAccessException("Failed to retrieve order.", ex);
        }
    }

    public async Task<Order?> Update(Order order)
    {
        try
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
        catch (DbUpdateException ex)
        {
            throw new DataAccessException("Failed to update order.", ex);
        }
    }

    public async Task<Order?> Delete(Guid id)
    {
        try
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) return null;

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
        catch (DbUpdateException ex)
        {
            throw new DataAccessException("Failed to delete order.", ex);
        }
    }


}
