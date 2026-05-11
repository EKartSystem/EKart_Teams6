using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EkartContext _context;

        public OrderRepository(EkartContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByCustomerAsync(string customerId)
        {
            return await _context.Orders
                .Where(x => x.CustomerId == customerId).ToListAsync();
        }

        //public async Task<IEnumerable<OrderDto>> GetByCustomerAsync(string customerId)
        //{
        //    var orders = await _context.Orders
        //        .FromSqlRaw(
        //            "EXEC CustOrdersOrders @CustomerID = {0}",
        //            customerId)
        //        .ToListAsync();

        //    return orders.Select(x => new OrderDto
        //    {
        //        OrderId = x.OrderId,
        //        OrderDate = x.OrderDate,
        //        RequiredDate = x.RequiredDate,
        //        ShippedDate = x.ShippedDate
        //    });
        //}

        public async Task<IEnumerable<Order>> GetRecentAsync(int count)
        {
            return await _context.Orders
                .OrderByDescending(x => x.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByShipperAsync(int shipperId)
        {
            return await _context.Orders
                .Where(x => x.ShipVia == shipperId)
                .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}