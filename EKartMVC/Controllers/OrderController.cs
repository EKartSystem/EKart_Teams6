using E_Kart_Application.DTOs.Orders;
using EKartMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderApiService _orderService;

        public OrderController(OrderApiService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            bool result = await _orderService.CreateOrderAsync(dto);
            if (result)
            {
                return RedirectToAction("Success");
            }
            ModelState.AddModelError("", "Order creation failed");
            return View(dto);
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderAsync(id);

            return View(order);
        }

        public async Task<IActionResult> MyOrders()
        {
            var orders = await _orderService.GetDetailsAsync();

            return View(orders);
        }
    }
}