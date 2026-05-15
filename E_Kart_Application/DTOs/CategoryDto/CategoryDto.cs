namespace E_Kart_Application.DTOs.CategoryDto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public int ProductCount { get; set; }
    }
    public class ResponseCategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string? Description { get; set; }
    }
}