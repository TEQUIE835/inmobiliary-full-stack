using inmobiliaryApi.Application.Services;
using inmobiliaryApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // 👈 Protege TODO el controller por defecto
public class PropertyController : ControllerBase
{
    private readonly PropertyService _service;

    public PropertyController(PropertyService service)
    {
        _service = service;
    }

    // GET: api/Property
    [HttpGet]
    public async Task<IActionResult> GetPropertiesAll()
    {
        var properties = await _service.GetAll();
        return Ok(properties);
    }

    // GET: api/Property/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPropertyById(int id)
    {
        var property = await _service.GetById(id);

        if (property == null)
            return NotFound(new { message = "Propiedad no encontrada" });

        return Ok(property);
    }

    // POST: api/Property
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProperty([FromBody] Property property)
    {
        await _service.CreateProperty(property);
        return Ok(new { message = "Propiedad creada correctamente" });
    }

    // PUT: api/Property/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProperty(int id, [FromBody] Property property)
    {
        if (property.Id != id)
        {
            return BadRequest(new { message = "El ID de la URL no coincide con el cuerpo" });
        }

        await _service.UpdateProperty(property);
        return Ok(property);
    }

    // DELETE: api/Property/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        var property = await _service.GetById(id);

        if (property == null)
            return NotFound(new { message = "Propiedad no encontrada" });

        await _service.DeleteProperty(property);
        return Ok(new { message = "Propiedad eliminada" });
    }
}
