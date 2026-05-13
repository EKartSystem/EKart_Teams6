using Microsoft.AspNetCore.Mvc;

public class CustomerController : Controller
{
    private readonly CustomerApiService _service;

    public CustomerController(CustomerApiService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Profile(string id)
    {
        var token = HttpContext.Session.GetString("jwt");

        if (token == null)
            return RedirectToAction("Login", "Account");

        var customer = await _service.GetProfile(id, token);

        return View(customer);
    }
}