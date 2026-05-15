using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly EKARTContext _context;

        public ShipperRepository(EKARTContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipper>> GetAllAsync()
        {
            return await _context.Shippers
                .OrderBy(s => s.ShipperId)
                .ToListAsync();
        }

        public async Task<Shipper?> GetByIdAsync(int id)
        {
            return await _context.Shippers.FindAsync(id);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperIdAsync(int shipperId)
        {
            return await _context.Orders
                .Where(o => o.ShipVia == shipperId)
                .Select(o => new OrderSummaryDto
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate,
                    ShippedDate = o.ShippedDate,
                    ShipName = o.ShipName,
                    ShipCity = o.ShipCity,
                    ShipCountry = o.ShipCountry
                })
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shipper>> SearchByNameAsync(string name)
        {
            return await _context.Shippers
                .Where(s => s.CompanyName.ToLower().Contains(name.ToLower()))
                .OrderBy(s => s.CompanyName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync()
        {
            return await _context.Shippers
                .Select(s => new ShipperWithOrderCountDto
                {
                    ShipperId = s.ShipperId,
                    CompanyName = s.CompanyName,
                    Phone = s.Phone,
                    OrderCount = _context.Orders.Count(o => o.ShipVia == s.ShipperId)
                })
                .OrderByDescending(x => x.OrderCount)
                .ToListAsync();
        }

        public async Task<Shipper> CreateAsync(Shipper shipper)
        {
            _context.Shippers.Add(shipper);
            await _context.SaveChangesAsync();
            return shipper;
        }
        public async Task<bool> UpdateAsync(Shipper shipper)
        {
            _context.Entry(shipper).State = EntityState.Modified;
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> UpdateNameAsync(int id, string newName)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null) return false;

            shipper.CompanyName = newName;
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> UpdatePhoneAsync(int id, string? newPhone)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null) return false;

            shipper.Phone = newPhone;
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Shippers.AnyAsync(s => s.ShipperId == id);
        }
    }
}