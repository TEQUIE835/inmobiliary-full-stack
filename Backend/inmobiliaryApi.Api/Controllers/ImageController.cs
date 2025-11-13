using inmobiliaryApi.Application.Services;
using inmobiliaryApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly ImageService _service;

    public ImageController(ImageService service)
    {
        _service = service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] int propertyId)
    {
        if (file == null)
            return BadRequest("Debe seleccionar una imagen.");
        var image = await _service.uploadImage(file, propertyId);
        return Ok(new { message = "Imagen subida correcta mente", image });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.deleteImage(id);
        if (!deleted)
            return NotFound("Imagen no encontrada.");
        return Ok(new { message = "Imagen eliminada correctamente" });
    }
}