namespace E_Kart_Application.DTOs.ProductsDTO
{
    public class ExpensiveProductDto
    {
        public string TenMostExpensiveProducts { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
