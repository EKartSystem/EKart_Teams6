namespace E_Kart_Application.DTOs.OrderDetails
{
    public class CreateOrderDetailDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; }
    }
}