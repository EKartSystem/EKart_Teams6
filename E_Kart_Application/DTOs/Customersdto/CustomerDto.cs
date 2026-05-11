namespace E_Kart_Application.DTOs.Customersdto
{
    public class CustomerDto
    {
        public string CustomerId { get; set; } = string.Empty;

        public string ContactName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string Role { get; set; } = string.Empty;

        public int TotalOrders { get; set; }
    }
}
