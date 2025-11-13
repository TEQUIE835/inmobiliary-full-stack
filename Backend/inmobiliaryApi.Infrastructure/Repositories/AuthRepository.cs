using inmobiliaryApi.Domain.Entities;
using inmobiliaryApi.Domain.Interfaces;
using inmobiliaryApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace inmobiliaryApi.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
    }

    public async Task DeleteRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
    }
}