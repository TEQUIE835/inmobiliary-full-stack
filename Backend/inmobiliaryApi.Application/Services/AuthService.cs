using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using inmobiliaryApi.Domain.Entities;
using inmobiliaryApi.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace inmobiliaryApi.Application.Services;

public class AuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IAuthRepository _authRepository;

    public AuthService(IUserRepository repository, IConfiguration configuration, IAuthRepository authRepository)
    {
        _repository = repository;
        _configuration = configuration;
        _authRepository = authRepository;
    }

    public async Task<bool> Register(string username, string password, string email)
    {
        var existingUser = await _repository.GetUserByUsername(username);
        if (existingUser != null)
            return false;

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Email = email
        };

        await _repository.CreateUser(user);
        return true;
    }

    public async Task<(string accessToken, string refreshToken)?> Authenticate(string username, string password)
    {
        var user = await _repository.GetUserByUsername(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;


        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: cred
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var expiration = DateTime.UtcNow.AddDays(7);

        var refreshEntity = new RefreshToken
        {
            Token = refreshToken,
            Expires = expiration,
            UserId = user.Id
        };

        await _authRepository.GetRefreshToken(refreshEntity.Token);


        return (accessToken, refreshToken);
    }

    public async Task<string?> RefreshAccessToken(string refreshToken)
    {
        var storedToken = await _authRepository.GetRefreshToken(refreshToken);
        if (storedToken == null || storedToken.Expires < DateTime.UtcNow)
            return null;

        var user = await _repository.GetUserById(storedToken.UserId);
        if (user == null)
            return null;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var newToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(newToken);
    }

    public async Task DeleteRefreshToken(string refreshToken)
    {
        var storedToken = await _authRepository.GetRefreshToken(refreshToken);
        if (storedToken != null)
            await _authRepository.DeleteRefreshToken(storedToken);
    }
}