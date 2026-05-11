using E_Kart_Application.DTOs;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersApiController : ControllerBase
{
    private readonly ISupplierService _service;

    public SuppliersApiController(ISupplierService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _service.GetAllAsync();
        return Ok(suppliers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _service.GetByIdAsync(id);
        if (supplier == null)
            throw new NotFoundException("Supplier", id);
        return Ok(supplier);
    }

    [HttpGet("{id:int}/products")]
    public async Task<IActionResult> GetProductsBySupplier(int id)
    {
        var supplier = await _service.GetByIdAsync(id);
        if (supplier == null)
            throw new NotFoundException("Supplier", id);
        var products = await _service.GetProductsBySupplierAsync(id);
        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Search term 'name' is required.");
        var results = await _service.SearchByNameAsync(name);
        return Ok(results);
    }

    [HttpGet("country/{country}")]
    public async Task<IActionResult> GetByCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
            throw new BadRequestException("Country is required.");
        var suppliers = await _service.GetByCountryAsync(country);
        return Ok(suppliers);
    }

    [HttpGet("with-product-count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetWithProductCount()
    {
        var result = await _service.GetWithProductCountAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid supplier data provided.");
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.SupplierId }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid supplier data provided.");
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            throw new NotFoundException("Supplier", id);
        return NoContent();
    }

    [HttpPatch("{id:int}/contact")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] PatchSupplierContactDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid contact data provided.");
        var updated = await _service.UpdateContactAsync(id, dto);
        if (!updated)
            throw new NotFoundException("Supplier", id);
        return NoContent();
    }

    [HttpPatch("{id:int}/address")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAddress(int id, [FromBody] PatchSupplierAddressDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid address data provided.");
        var updated = await _service.UpdateAddressAsync(id, dto);
        if (!updated)
            throw new NotFoundException("Supplier", id);
        return NoContent();
    }
}
