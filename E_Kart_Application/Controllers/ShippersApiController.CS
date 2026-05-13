using E_Kart_Application.DTOs;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Kart_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippersApiController : ControllerBase
{
    private readonly IShipperService _service;

    public ShippersApiController(IShipperService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var shippers = await _service.GetAllAsync();
        return Ok(shippers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var shipper = await _service.GetByIdAsync(id);
        if (shipper == null)
            throw new NotFoundException("Shipper", id);
        return Ok(shipper);
    }

    [HttpGet("{id:int}/orders")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrdersByShipper(int id)
    {
        var shipper = await _service.GetByIdAsync(id);
        if (shipper == null)
            throw new NotFoundException("Shipper", id);
        var orders = await _service.GetOrdersByShipperAsync(id);
        return Ok(orders);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Search term 'name' is required.");
        var results = await _service.SearchByNameAsync(name);
        return Ok(results);
    }

    [HttpGet("with-order-count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetWithOrderCount()
    {
        var result = await _service.GetWithOrderCountAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] ShipperRequestDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid shipper data provided.");
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.ShipperId }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] ShipperRequestDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid shipper data provided.");
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            throw new NotFoundException("Shipper", id);
        return NoContent();
    }

    [HttpPatch("{id:int}/name")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateName(int id, [FromBody] PatchShipperNameDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid name provided.");
        var updated = await _service.UpdateNameAsync(id, dto);
        if (!updated)
            throw new NotFoundException("Shipper", id);
        return NoContent();
    }

    [HttpPatch("{id:int}/phone")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePhone(int id, [FromBody] PatchShipperPhoneDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid phone provided.");
        var updated = await _service.UpdatePhoneAsync(id, dto);
        if (!updated)
            throw new NotFoundException("Shipper", id);
        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] PatchShipperStatusDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid status provided.");
        var updated = await _service.UpdateStatusAsync(id, dto);
        if (!updated)
            throw new NotFoundException("Shipper", id);
        return NoContent();
    }
}