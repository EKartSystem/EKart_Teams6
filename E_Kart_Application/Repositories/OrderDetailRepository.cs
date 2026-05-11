using E_Kart_Application.DBContext;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly EkartContext _context;

        public OrderDetailRepository(EkartContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .ToListAsync();
        }

        public async Task<OrderDetail?> GetByIdAsync(int orderId, int productId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);
        }

        public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return await _context.OrderDetails
                .Include(od => od.Product)
                .FirstAsync(od =>
                    od.OrderId == orderDetail.OrderId &&
                    od.ProductId == orderDetail.ProductId);
        }

        public async Task<OrderDetail?> UpdateAsync(OrderDetail orderDetail)
        {
            var existingOrderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderDetail.OrderId && od.ProductId == orderDetail.ProductId);
            if (existingOrderDetail == null)
            {
                return null;
            }
            existingOrderDetail.UnitPrice = orderDetail.UnitPrice;
            existingOrderDetail.Quantity = orderDetail.Quantity;
            existingOrderDetail.Discount = orderDetail.Discount;
            await _context.SaveChangesAsync();
            return existingOrderDetail;
        }

        public async Task<bool> DeleteAsync(int orderId, int productId)
        {
            var existingOrderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);
            if (existingOrderDetail == null)
            {
                return false;
            }
            _context.OrderDetails.Remove(existingOrderDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}