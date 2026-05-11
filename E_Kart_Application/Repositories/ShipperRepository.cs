using E_Kart_Application.DBContext;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Kart_Application.Repositories
{
    // This is the ACTUAL implementation. It talks directly to the database
    // using EKARTFullContext (Entity Framework Core).
    //
    // Every method here runs a real SQL query under the hood.
    public class ShipperRepository : IShipperRepository
    {
        private readonly EKARTContext _context;

        // EKARTFullContext is injected via constructor (Dependency Injection)
        public ShipperRepository(EKARTContext context)
        {
            _context = context;
        }

        // GET /api/shippers
        // Fetches ALL shippers from the Shippers table
        public async Task<IEnumerable<Shipper>> GetAllAsync()
        {
            return await _context.Shippers
                .OrderBy(s => s.CompanyName)
                .ToListAsync();
        }

        // GET /api/shippers/{id}
        // Finds exactly one shipper by primary key — returns null if ID doesn't exist
        public async Task<Shipper?> GetByIdAsync(int id)
        {
            return await _context.Shippers.FindAsync(id);
        }

		// GET /api/shippers/{id}/orders
		// Returns all orders where ShipVia column matches this ShipperID
		// ShipVia is the foreign key in the Orders table that points to ShipperID
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

		// GET /api/shippers/search?name=xyz
		// Performs a case-insensitive LIKE search on CompanyName
		// SQL equivalent: WHERE CompanyName LIKE '%xyz%'
		public async Task<IEnumerable<Shipper>> SearchByNameAsync(string name)
        {
            return await _context.Shippers
                .Where(s => s.CompanyName.ToLower().Contains(name.ToLower()))
                .OrderBy(s => s.CompanyName)
                .ToListAsync();
        }

        // GET /api/shippers/with-order-count
        // Returns each shipper + count of orders they handled
        // SQL equivalent: SELECT s.*, COUNT(o.OrderID) ... GROUP BY s.ShipperID
        public async Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync()
        {
            return await _context.Shippers
                .Select(s => new ShipperWithOrderCountDto
                {
                    ShipperId   = s.ShipperId,
                    CompanyName = s.CompanyName,
                    Phone       = s.Phone,
                    OrderCount  = _context.Orders.Count(o => o.ShipVia == s.ShipperId)
                })
                .OrderByDescending(x => x.OrderCount)
                .ToListAsync();
        }

        // POST /api/shippers
        // Inserts a new row into the Shippers table
        public async Task<Shipper> CreateAsync(Shipper shipper)
        {
            _context.Shippers.Add(shipper);
            await _context.SaveChangesAsync();
            return shipper; // EF Core now has the auto-generated ShipperID on this object
        }

        // PUT /api/shippers/{id}
        // Replaces all fields of an existing shipper (full update)
        public async Task<bool> UpdateAsync(Shipper shipper)
        {
            _context.Entry(shipper).State = EntityState.Modified;
            var rows = await _context.SaveChangesAsync();
            return rows > 0; // returns true if at least one row was changed
        }

        // PATCH /api/shippers/{id}/name
        // Only changes CompanyName — all other fields stay untouched
        public async Task<bool> UpdateNameAsync(int id, string newName)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null) return false;

            shipper.CompanyName = newName;
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        // PATCH /api/shippers/{id}/phone
        // Only changes Phone — all other fields stay untouched
        public async Task<bool> UpdatePhoneAsync(int id, string? newPhone)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null) return false;

            shipper.Phone = newPhone;
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        // PATCH /api/shippers/{id}/status
        // Soft enable/disable. Does NOT delete the record.
        // WHY: old Orders reference ShipperID — deleting breaks those records (FK violation)
        //public async Task<bool> UpdateStatusAsync(int id, bool isActive)
        //{
        //    var shipper = await _context.Shippers.FindAsync(id);
        //    if (shipper == null) return false;

        //    // NOTE: IsActive is a custom field your team needs to add to the Shipper model
        //    // and run a migration for. The base Northwind table does not have it.
        //    //shipper.IsActive = isActive;
        //    var rows = await _context.SaveChangesAsync();
        //    return rows > 0;
        //}

        // Helper — used to quickly check if a shipper exists before doing operations
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Shippers.AnyAsync(s => s.ShipperId == id);
        }
    }
}
