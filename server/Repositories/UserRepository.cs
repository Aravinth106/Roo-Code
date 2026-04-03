using MongoDB.Driver;
using RooCode.Chat.Server.Data;
using RooCode.Chat.Server.Models;

namespace RooCode.Chat.Server.Repositories;

public class UserRepository(MongoDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email)
        => dbContext.Users.Find(x => x.Email == email.ToLowerInvariant()).FirstOrDefaultAsync();

    public Task<User?> GetByIdAsync(string id)
        => dbContext.Users.Find(x => x.Id == id).FirstOrDefaultAsync();

    public Task<List<User>> GetAllExceptAsync(string userId)
        => dbContext.Users.Find(x => x.Id != userId).SortBy(x => x.Name).ToListAsync();

    public Task CreateAsync(User user) => dbContext.Users.InsertOneAsync(user);
}
