using System.ComponentModel.DataAnnotations;

namespace E_Kart_Application.DTOs
{
    public class PatchShipperNameDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(40, ErrorMessage = "Company name cannot exceed 40 characters")]
        public string CompanyName { get; set; } = string.Empty;
    }

    public class PatchShipperPhoneDto
    {
        [StringLength(24, ErrorMessage = "Phone cannot exceed 24 characters")]
        public string? Phone { get; set; }
    }

    public class PatchShipperStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }

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
