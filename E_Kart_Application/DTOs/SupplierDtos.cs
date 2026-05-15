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

public class SupplierRequestDto
{
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

public class PatchSupplierContactDto
{
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
}

public class PatchSupplierAddressDto
{
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
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