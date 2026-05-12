using Azure;
using E_Kart_Application.DTOs.Customersdto;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly CustomerApiService _service;

    public AccountController(CustomerApiService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(CustomerLogin dto)
    {
        var token = await _service.LoginAsync(dto);

        if (token == null)
        {
            ViewBag.Error = "Invalid Credentials";
            return View();
        }

        HttpContext.Session.SetString("jwt", token);
        if (dto.ContactName == "Maria Anders") 
        {
            HttpContext.Session.SetString("IsAdmin", "true");
        }
        else
        {
            HttpContext.Session.SetString("IsAdmin", "false");
        }

        return RedirectToAction("Index","Shop");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Remove("jwt");
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCustomerDto dto)
    {
        var result = await _service.RegisterAsync(dto);

        if (!result)
        {
            ViewBag.Error = "Registration Failed";
            return View();
        }

        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Profile()
    {
        var token = Request.Cookies["jwt"];

        if (token == null)
            return RedirectToAction("Login");

        var customer = await _service.GetProfileAsync("ALFKI", token);
        return View(customer);
    }
}