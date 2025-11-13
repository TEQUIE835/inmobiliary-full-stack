using inmobiliaryApi.Application.Services;
using inmobiliaryApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly PropertyService _service;

    public PropertyController(PropertyService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPropertiesAll()
    {
        var property = await _service.GetAll();
        return Ok(property);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPropertyById(int id)
    {
        var property = await _service.GetById(id);
        return Ok(property);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProperty(Property property)
    {
        await _service.CreateProperty(property);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{property}")]
    public async Task<IActionResult> UpdateProperty(Property property)
    {
        await _service.UpdateProperty(property);
        return Ok(property);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{property}")]
    public async Task<IActionResult> DeleteProperty(Property property)
    {
        await _service.DeleteProperty(property);
        return Ok();
    }
}