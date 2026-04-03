using MongoDB.Driver;
using RooCode.Chat.Server.Data;
using RooCode.Chat.Server.Models;

namespace RooCode.Chat.Server.Repositories;

public class MessageRepository(MongoDbContext dbContext) : IMessageRepository
{
    public Task<List<Message>> GetConversationAsync(string userId, string otherUserId)
        => dbContext.Messages
            .Find(x =>
                (x.SenderId == userId && x.ReceiverId == otherUserId) ||
                (x.SenderId == otherUserId && x.ReceiverId == userId))
            .SortBy(x => x.Timestamp)
            .ToListAsync();

    public async Task<Message> CreateAsync(Message message)
    {
        await dbContext.Messages.InsertOneAsync(message);
        return message;
    }
}
