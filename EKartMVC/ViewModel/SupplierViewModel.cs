using System.ComponentModel.DataAnnotations;

namespace EKartMVC.Models;

public class SupplierViewModel
{
    public int SupplierId { get; set; }

    [Required(ErrorMessage = "Company name is required")]
    [StringLength(40)]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [StringLength(30)]
    [Display(Name = "Contact Name")]
    public string? ContactName { get; set; }

    [StringLength(30)]
    [Display(Name = "Contact Title")]
    public string? ContactTitle { get; set; }

    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(15)]
    public string? City { get; set; }

    [StringLength(15)]
    public string? Region { get; set; }

    [StringLength(10)]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    [StringLength(15)]
    public string? Country { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(24)]
    public string? Fax { get; set; }

    [Display(Name = "Home Page")]
    public string? HomePage { get; set; }
}

public class SupplierWithProductCountViewModel
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public int ProductCount { get; set; }
}
