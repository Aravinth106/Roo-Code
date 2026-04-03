using RooCode.Chat.Server.Models;

namespace RooCode.Chat.Server.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(string id);
    Task<List<User>> GetAllExceptAsync(string userId);
    Task CreateAsync(User user);
}
