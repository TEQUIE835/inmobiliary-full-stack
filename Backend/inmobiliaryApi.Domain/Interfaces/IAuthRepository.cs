using inmobiliaryApi.Domain.Entities;

namespace inmobiliaryApi.Domain.Interfaces;

public interface IAuthRepository
{
    public Task<RefreshToken?> GetRefreshToken(string refreshToken);
    public Task DeleteRefreshToken(RefreshToken refreshToken);
    
}