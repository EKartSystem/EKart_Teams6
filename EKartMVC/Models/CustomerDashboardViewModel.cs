using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.Customersdto;

namespace EKartMVC.Models
{
    public class CustomerDashboardViewModel
    {
        public CustomerDto? Customer { get; set; }

        public List<OrderDto> Orders { get; set; } = new();

        public int TotalOrders => Orders.Count;




    }
}
