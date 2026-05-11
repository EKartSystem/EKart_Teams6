namespace E_Kart_Application.DTOs.OrderDetails
{
    public class OrderDetailResponseDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public string? ProductName { get; set; }
    }
}
