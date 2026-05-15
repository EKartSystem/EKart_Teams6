using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.DTOs.OrderDetails;

namespace EKartMVC.ViewModel
{
    public class OrderDetailsViewModel
    {
        public OrderDto Order { get; set; }
        public List<OrderDetailResponseDto> OrderDetails { get; set; }

        public OrderDetailsViewModel()
        {
            OrderDetails = new List<OrderDetailResponseDto>();
        }

        public OrderDetailsViewModel(OrderDto order, List<OrderDetailResponseDto> orderDetails)
        {
            Order = order;
            OrderDetails = orderDetails;
        }

        public decimal GetTotalPrice()
        {
            return OrderDetails.Sum(od => od.Quantity * od.UnitPrice);
        }
    }
}
