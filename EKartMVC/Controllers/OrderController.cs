using EKartMVC.Services;
using EKartMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EKartMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderApiService _orderService;
        private readonly CustomerApiService _customerApiService;
        private readonly AdminServices _adminService;
        private readonly ShipperApiService _shipperService;

        public OrderController(OrderApiService orderService, CustomerApiService customerApiService, AdminServices adminService, ShipperApiService shipperService)
        {
            _orderService = orderService;
            _customerApiService = customerApiService;
            _adminService = adminService;
            _shipperService = shipperService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var token = HttpContext.Session.GetString("jwt");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(token) || role != "Admin")
                return RedirectToAction("AccessDenied", "Account");

            var vm = await _orderService.GetPagedOrdersAsync(pageNumber, 10, token);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cartService = new CartService(HttpContext.Session);
            var cart = cartService.GetCart();

            if (cart.Count == 0)
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }

            var token = HttpContext.Session.GetString("jwt");
            var dto = await _orderService.BuildCheckoutDtoAsync(cart, token, _customerApiService);

            if (dto == null)
                return RedirectToAction("Login", "Account");

            ViewBag.CartItems = cart;
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(E_Kart_Application.DTOs.Orders.CreateOrderDto dto)
        {
            var token = HttpContext.Session.GetString("jwt");

            if (string.IsNullOrEmpty(dto.CustomerId))
                dto.CustomerId = _orderService.GetCustomerIdFromToken(token);

            if (!ModelState.IsValid)
            {
                ViewBag.CartItems = new CartService(HttpContext.Session).GetCart();
                return View(dto);
            }

            var result = await _orderService.CreateOrderAsync(dto, token);

            if (result)
            {
                var cartService = new CartService(HttpContext.Session);
                TempData["OrderItems"] = System.Text.Json.JsonSerializer.Serialize(cartService.GetCart());
                TempData["RequiredDate"] = dto.RequiredDate?.ToString("yyyy-MM-dd");
                cartService.ClearCart();
                return RedirectToAction(nameof(Success));
            }

            return RedirectToAction(nameof(Failure));
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("jwt");
            var order = await _orderService.GetOrderAsync(id, token);
            if (order == null) return NotFound();

            var orderDetails = await _orderService.GetOrderDetailsByOrderIdAsync(id, token);

            if (HttpContext.Session.GetString("role") == "Admin")
            {
                ViewBag.Employees = await _adminService.GetEmployeeAll(token);
                ViewBag.Shippers = await _shipperService.GetAllShippersAsync(token);
            }

            return View(new OrderDetailsViewModel(order, orderDetails));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFulfillment(int id, int? employeeId, int? shipVia, decimal? freight)
        {
            var token = HttpContext.Session.GetString("jwt");
            var success = await _orderService.UpdateFulfillmentAsync(id, employeeId, shipVia, freight, token);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, bool markShipped)
        {
            var token = HttpContext.Session.GetString("jwt");
            await _orderService.UpdateOrderStatusAsync(id, markShipped ? DateTime.UtcNow : null, token);
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> MyOrders(string customerId)
        {
            var token = HttpContext.Session.GetString("jwt");

            if (string.IsNullOrEmpty(customerId))
                customerId = _orderService.GetCustomerIdFromToken(token);

            if (string.IsNullOrEmpty(customerId))
            {
                TempData["Error"] = "Please log in to view your orders.";
                return RedirectToAction("Login", "Account");
            }

            var viewModels = await _orderService.GetCustomerOrderViewModelsAsync(customerId, token);
            return View(viewModels);
        }

        public IActionResult Success() => View();
        public IActionResult Failure() => View();
    }
}
