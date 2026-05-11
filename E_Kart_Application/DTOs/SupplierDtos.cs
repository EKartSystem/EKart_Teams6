using System.ComponentModel.DataAnnotations;

namespace E_Kart_Application.DTOs;

public class SupplierDto
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? HomePage { get; set; }
}

public class CreateSupplierDto
{
    [Required(ErrorMessage = "Company name is required")]
    [StringLength(40)]
    public string CompanyName { get; set; } = string.Empty;

    [StringLength(30)]
    public string? ContactName { get; set; }

    [StringLength(30)]
    public string? ContactTitle { get; set; }

    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(15)]
    public string? City { get; set; }

    [StringLength(15)]
    public string? Region { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(15)]
    public string? Country { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(24)]
    public string? Fax { get; set; }

    public string? HomePage { get; set; }
}

public class UpdateSupplierDto
{
    [Required(ErrorMessage = "Company name is required")]
    [StringLength(40)]
    public string CompanyName { get; set; } = string.Empty;

    [StringLength(30)]
    public string? ContactName { get; set; }

    [StringLength(30)]
    public string? ContactTitle { get; set; }

    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(15)]
    public string? City { get; set; }

    [StringLength(15)]
    public string? Region { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(15)]
    public string? Country { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(24)]
    public string? Fax { get; set; }

    public string? HomePage { get; set; }
}

public class PatchSupplierContactDto
{
    [StringLength(30)]
    public string? ContactName { get; set; }

    [StringLength(30)]
    public string? ContactTitle { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(24)]
    public string? Fax { get; set; }
}

public class PatchSupplierAddressDto
{
    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(15)]
    public string? City { get; set; }

    [StringLength(15)]
    public string? Region { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(15)]
    public string? Country { get; set; }
}

public class SupplierWithProductCountDto
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public int ProductCount { get; set; }
}

public class ProductSummaryDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? QuantityPerUnit { get; set; }
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
}
