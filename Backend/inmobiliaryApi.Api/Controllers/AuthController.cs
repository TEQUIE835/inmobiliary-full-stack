using inmobiliaryApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaryApi.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await _service.RefreshAccessToken(request.RefreshToken);
        if (result == null)
            return Unauthorized(new { message = "Token invalido o expirado." });
        return Ok(result);
    }

    public record RefreshTokenRequest(string RefreshToken);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _service.Authenticate(request.Email, request.Password);
        if (token == null)
        {
            return Unauthorized(new { message = "El usuario o la contraseña no coinciden." });
        }

        return Ok(token);
    }

    public record LoginRequest(string Email, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var success = await _service.Register(request.Username, request.Password, request.Email);
        if (!success)
            return BadRequest(new { message = "El usuario ya existe." });
        return Ok(new { message = "El usuario se ha registrado exitosamente." });
    }

    public record RegisterRequest(string Username, string Password, string Email);

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest(new { message = "El token de actualizacion es requerido." });

        await _service.DeleteRefreshToken(refreshToken);
        return Ok(new { message = "Se ha cerrado la seccion correctamente." });
    }
}