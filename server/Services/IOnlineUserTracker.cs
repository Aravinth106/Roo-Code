namespace RooCode.Chat.Server.Services;

public interface IOnlineUserTracker
{
    void AddOrUpdate(string userId, string connectionId);
    void RemoveByConnectionId(string connectionId);
    string? GetConnectionId(string userId);
    IReadOnlyCollection<string> GetOnlineUsers();
}
