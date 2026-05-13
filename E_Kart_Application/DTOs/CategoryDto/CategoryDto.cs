namespace E_Kart_Application.DTOs.CategoryDto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public int ProductCount { get; set; }
    }

    public class ProductListingDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public decimal? UnitPrice { get; set; }

        public string? QuantityPerUnit { get; set; }

        public bool IsInStock { get; set; }
    }
    public class ResponseCategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string? Description { get; set; }
    }
}