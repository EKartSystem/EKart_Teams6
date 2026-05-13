using E_Kart_Application.DTOs.Customersdto;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EKartMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly CustomerApiService _service;

        public AdminController(CustomerApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var data = await _service.GetCustomersAsync(token);

            return View(data ?? new List<CustomerDto>());
        }

        public IActionResult AdminDashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            return View();
        }

        public async Task<IActionResult> Profile()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var token = GetToken();
            var userId = GetUserIdFromSessionOrToken();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var admin = await _service.GetProfileAsync(userId, token);

                if (admin != null)
                    return View(admin);
            }

            return View(new CustomerDto
            {
                ContactName = HttpContext.Session.GetString("username") ?? "Admin",
                Role = HttpContext.Session.GetString("role") ?? "Admin"
            });
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
