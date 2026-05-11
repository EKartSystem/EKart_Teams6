namespace E_Kart_Application.DTOs.OrderDetails
{
    public class UpdateOrderDetailDto
    {
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    }
}
