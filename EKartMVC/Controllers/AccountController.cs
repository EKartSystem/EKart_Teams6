using E_Kart_Application.DTOs.Customersdto;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly CustomerApiService _service;

    public AccountController(CustomerApiService service)
    {
        _service = service;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(CustomerLogin dto)
    {
        var token = await _service.LoginAsync(dto);

        if (token == null)
        {
            ViewBag.Error = "Invalid Credentials";
            return View();
        }

        HttpContext.Session.SetString("jwt", token);
        HttpContext.Session.SetString("username", dto.ContactName ?? "User");

        if (dto.ContactName == "Maria Anders")
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("role", "Admin");
            return RedirectToAction("AdminDashboard", "Admin");
        }
        else
        {
            HttpContext.Session.SetString("IsAdmin", "false");
            HttpContext.Session.SetString("role", "Customer");
            return RedirectToAction("Index", "Home");

        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterCustomerDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _service.RegisterAsync(dto);

        if (!result)
        {
            ViewBag.Error = "Registration Failed";
            return View(dto);
        }

        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
