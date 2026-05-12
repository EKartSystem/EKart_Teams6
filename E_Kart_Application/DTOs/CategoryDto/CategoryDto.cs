namespace E_Kart_Application.DTOs.CategoryDto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }
    }
    public class CreateCategoryDto
    {
        public string? CategoryName { get; set; }

        public string? Description { get; set; }
    }
    public class UpdateCategoryDto
    {
        public string? CategoryName { get; set; }

        public string? Description { get; set; }
    }
    public class CategoryWithProductCountDto
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int ProductCount { get; set; }
    }
    public class UpdateCategoryDescriptionDto
    {
        public string? Description { get; set; }
    }
public class UpdateCategoryNameDto
    {
        public string? CategoryName { get; set; }
    }
}
