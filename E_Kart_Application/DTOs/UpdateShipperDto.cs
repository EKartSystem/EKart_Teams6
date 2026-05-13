using System.ComponentModel.DataAnnotations;

namespace E_Kart_Application.DTOs
{
    public class UpdateShipperDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(40, ErrorMessage = "Company name cannot exceed 40 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(24, ErrorMessage = "Phone cannot exceed 24 characters")]
        public string? Phone { get; set; }
    }
}
