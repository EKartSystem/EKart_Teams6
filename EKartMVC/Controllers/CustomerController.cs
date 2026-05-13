using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EKartMVC.Models;

namespace EKartMVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerApiService _service;

        public CustomerController(CustomerApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> CustomerDashboard()
        {
            if (!IsCustomer())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var userId = GetUserIdFromSessionOrToken();

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var customer = await _service.GetProfileAsync(userId, token);
            var orders = await _service.GetOrdersAsync(userId, token);

            var model = new CustomerDashboardViewModel
            {
                Customer = customer,
                Orders = orders ?? new List<E_Kart_Application.DTOs.OrderDto>()
            };

            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            if (!IsCustomer())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var userId = GetUserIdFromSessionOrToken();

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var data = await _service.GetProfileAsync(userId, token);

            if (data == null)
                return RedirectToAction("CustomerDashboard");

            return View(data);
        }

        public async Task<IActionResult> Orders()
        {
            if (!IsCustomer())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var userId = GetUserIdFromSessionOrToken();

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var orders = await _service.GetOrdersAsync(userId, token);

            return View(orders ?? new List<E_Kart_Application.DTOs.OrderDto>());
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("jwt") ?? string.Empty;
        }

        private bool IsCustomer()
        {
            var role = HttpContext.Session.GetString("role");
            return string.Equals(role, "Customer", StringComparison.OrdinalIgnoreCase);
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
