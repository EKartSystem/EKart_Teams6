namespace E_Kart_Application.DTOs.ProductsDTO
{
    public class ProductListingDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public string? QuantityPerUnit { get; set; }
        public bool IsInStock { get; set; }
    }
}
