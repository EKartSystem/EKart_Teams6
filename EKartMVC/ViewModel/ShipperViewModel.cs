using System.ComponentModel.DataAnnotations;

namespace EKartMVC.Models;

public class ShipperViewModel
{
    public int ShipperId { get; set; }

    [Required(ErrorMessage = "Company name is required")]
    [StringLength(40, ErrorMessage = "Company name cannot exceed 40 characters")]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [StringLength(24, ErrorMessage = "Phone cannot exceed 24 characters")]
    [Display(Name = "Phone Number")]
    public string? Phone { get; set; }
}

public class ShipperWithOrderCountViewModel
{
    public int ShipperId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int OrderCount { get; set; }
}
