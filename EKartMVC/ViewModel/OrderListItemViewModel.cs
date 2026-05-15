using E_Kart_Application.DTOs.Orders;

namespace EKartMVC.ViewModel
{
    public class OrderListItemViewModel
    {
        public OrderDto Order { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderListItemViewModel(OrderDto order, decimal totalPrice)
        {
            Order = order;
            TotalPrice = totalPrice;
        }
    }
}
