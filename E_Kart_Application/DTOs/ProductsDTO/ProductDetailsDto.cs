namespace E_Kart_Application.DTOs.ProductsDTO
{
    public class ProductDetailsDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; } 
        public string? CategoryName { get; set; } 
        public string? CategoryDescription { get; set; }
        public string? SupplierCompanyName { get; set; }
        public string? SupplierCountry { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool IsInStock { get; set; }
        public bool Discontinued { get; set; }
    }
}
