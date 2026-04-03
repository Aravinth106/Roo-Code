using RooCode.Chat.Server.Models;

namespace RooCode.Chat.Server.Repositories;

public interface IMessageRepository
{
    Task<List<Message>> GetConversationAsync(string userId, string otherUserId);
    Task<Message> CreateAsync(Message message);
}
