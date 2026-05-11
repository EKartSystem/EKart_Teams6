using E_Kart_Application.DTOs;
using E_Kart_Application.Models;

namespace E_Kart_Application.Repositories
{
    // This is the CONTRACT (interface) for all database operations on Shippers.
    // The actual implementation (ShipperRepository) does the real EF Core queries.
    //
    // WHY use an interface?
    // → Makes the code testable (you can swap in a fake repo in unit tests)
    // → Follows the Dependency Inversion principle (SOLID)
    public interface IShipperRepository
    {
        // GET all shippers
        Task<IEnumerable<Shipper>> GetAllAsync();

        // GET one shipper by ID — returns null if not found
        Task<Shipper?> GetByIdAsync(int id);

        // GET all orders that belong to a specific shipper
        Task<IEnumerable<OrderSummaryDto>> GetOrdersByShipperIdAsync(int shipperId);
        // GET shippers whose name contains the search term
        Task<IEnumerable<Shipper>> SearchByNameAsync(string name);

        // GET shippers with their order count (for the analytics endpoint)
        Task<IEnumerable<ShipperWithOrderCountDto>> GetWithOrderCountAsync();

        // POST — add a new shipper to the DB
        Task<Shipper> CreateAsync(Shipper shipper);

        // PUT — update all fields of an existing shipper
        Task<bool> UpdateAsync(Shipper shipper);

        // PATCH — update only CompanyName
        Task<bool> UpdateNameAsync(int id, string newName);

        // PATCH — update only Phone
        Task<bool> UpdatePhoneAsync(int id, string? newPhone);

        // PATCH — activate or deactivate a shipper (soft delete/enable)
        Task<bool> UpdateStatusAsync(int id, bool isActive);

        // Helper used by other methods to check if a shipper exists
        Task<bool> ExistsAsync(int id);
    }
}
