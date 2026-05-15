using E_Kart_Application.DTOs.Customersdto;
using EKartMVC.Services;
using EKartMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EKartMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly CustomerApiService _service;
        private readonly OrderApiService _orderService;
        private readonly ProductApiService _productService;

        public AdminController(CustomerApiService service, OrderApiService orderService, ProductApiService productService)
        {
            _service = service;
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var data = await _service.GetCustomersAsync(token);

            return View(data ?? new List<CustomerDto>());
        }

        public async Task<IActionResult> AdminDashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();

            var customers = await _service.GetCustomersAsync(token);
            var orders = await _orderService.GetAllOrdersAsync(token);
            var products = await _productService.GetPagedDataAsync(1, int.MaxValue, token);

            var vm = new AdminDashboardViewModel
            {
                TotalCustomers = customers?.Count() ?? 0,
                TotalOrders = orders?.Count ?? 0,
                TotalProducts = products?.TotalCount ?? 0,
                PendingShipments = orders?.Count(o => o.ShippedDate == null) ?? 0
            };

            return View(vm);
        }

        public async Task<IActionResult> Profile()
        {
            var token = GetToken();
            var userId = GetUserIdFromSessionOrToken();

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var user = await _service.GetProfileAsync(userId, token);

            return View(user);
        }


        public async Task<IActionResult> Customers()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var data = await _service.GetCustomersAsync(token);

            return View(data ?? new List<CustomerDto>());
        }

        public async Task<IActionResult> Search(string? name)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var data = string.IsNullOrWhiteSpace(name)
                ? await _service.GetCustomersAsync(token)
                : await _service.SearchAsync(name, token);

            return View("Index", data ?? new List<CustomerDto>());
        }

        public async Task<IActionResult> TopCustomers()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var data = await _service.GetTopAsync(token);

            return View(data ?? new List<CustomerDto>());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var customer = await _service.GetProfileAsync(id, token);

            if (customer == null)
                return RedirectToAction("Index");

            var dto = new UpdateCustomerDto
            {
                ContactName = customer.ContactName,
                Phone = customer.Phone,
                Address = customer.Address
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateCustomerDto dto)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            if (!ModelState.IsValid)
                return View(dto);

            var token = GetToken();
            var success = await _service.UpdateCustomerAsync(id, dto, token);

            if (success)
                return RedirectToAction("Index");

            ViewBag.Error = "Unable to update customer.";
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var token = GetToken();
            var userId = GetUserIdFromSessionOrToken();

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var customer = await _service.GetProfileAsync(userId, token);

            if (customer == null)
                return RedirectToAction("Profile");

            var dto = new UpdateCustomerDto
            {
                ContactName = customer.ContactName,
                Phone = customer.Phone,
                Address = customer.Address
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UpdateCustomerDto dto)
        {
            var token = GetToken();
            var userId = GetUserIdFromSessionOrToken();

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(dto);

            var success = await _service.UpdateCustomerAsync(userId, dto, token);

            if (success)
                return RedirectToAction("Profile");

            ViewBag.Error = "Unable to update profile.";
            return View(dto);
        }
        private string GetToken()
        {
            return HttpContext.Session.GetString("jwt") ?? string.Empty;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("role");
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        private string? GetUserIdFromSessionOrToken()
        {
            var sessionUserId = HttpContext.Session.GetString("userId");

            if (!string.IsNullOrWhiteSpace(sessionUserId))
                return sessionUserId;

            var token = GetToken();

            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "nameid" ||
                c.Type == "sub" ||
                c.Type == "customerId" ||
                c.Type == "CustomerId" ||
                c.Type == "unique_name")?.Value;
        }
    }
}