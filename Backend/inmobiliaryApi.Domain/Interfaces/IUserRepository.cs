using inmobiliaryApi.Domain.Entities;

namespace inmobiliaryApi.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserByUsername(string username);
    public Task<User?> GetUserByEmail(string email);
    public Task<User?> GetUserById(int id);
    public Task CreateUser(User user);
}