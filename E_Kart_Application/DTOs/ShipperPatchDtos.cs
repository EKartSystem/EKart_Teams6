using System.ComponentModel.DataAnnotations;

namespace E_Kart_Application.DTOs
{
    // Used for PATCH /api/shippers/{id}/name
    // PATCH = update ONLY ONE field. Just the name. Phone stays unchanged.
    public class PatchShipperNameDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(40, ErrorMessage = "Company name cannot exceed 40 characters")]
        public string CompanyName { get; set; } = string.Empty;
    }

    // Used for PATCH /api/shippers/{id}/phone
    // PATCH = update ONLY ONE field. Just the phone. Name stays unchanged.
    public class PatchShipperPhoneDto
    {
        [StringLength(24, ErrorMessage = "Phone cannot exceed 24 characters")]
        public string? Phone { get; set; }
    }

    // Used for PATCH /api/shippers/{id}/status
    // Soft-enable or disable a shipper (safer than deleting — old orders still reference it)
    public class PatchShipperStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }

    // Used for GET /api/shippers/with-order-count response
    // This is a special read-only DTO — shipper + how many orders they handled
    public class ShipperWithOrderCountDto
    {
        public int ShipperId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int OrderCount { get; set; }
    }

    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? ShipName { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipCountry { get; set; }
    }
}
