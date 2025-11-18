using System.Threading.Tasks;
using inmobiliaryApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // DTOs simples para las peticiones
    public record RegisterRequest(string Username, string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record RefreshRequest(string RefreshToken);

    // POST: api/Auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Todos los campos son obligatorios.");
        }

        var registered = await _authService.Register(
            request.Username,
            request.Password,
            request.Email
        );

        if (!registered)
        {
            return BadRequest("El username o el email ya están en uso.");
        }

        return Ok(new { message = "Usuario registrado correctamente." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Correo y contraseña son obligatorios.");
        }

        
        var tokens = await _authService.Authenticate(request.Email, request.Password);

        if (tokens is null)
        {
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        return Ok(new
        {
            accessToken = tokens.Value.accessToken,
            refreshToken = tokens.Value.refreshToken
        });
    }


    // POST: api/Auth/refresh
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest("El refresh token es obligatorio.");

        var newAccessToken = await _authService.RefreshAccessToken(request.RefreshToken);

        if (newAccessToken is null)
            return Unauthorized("Refresh token inválido o expirado.");

        return Ok(new { accessToken = newAccessToken });
    }

    // POST: api/Auth/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            await _authService.DeleteRefreshToken(request.RefreshToken);
        }

        return Ok(new { message = "Sesión cerrada correctamente." });
    }
}
