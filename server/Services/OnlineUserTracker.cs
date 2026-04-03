using System.Collections.Concurrent;

namespace RooCode.Chat.Server.Services;

public class OnlineUserTracker : IOnlineUserTracker
{
    private readonly ConcurrentDictionary<string, string> _userConnections = new();

    public void AddOrUpdate(string userId, string connectionId)
        => _userConnections.AddOrUpdate(userId, connectionId, (_, _) => connectionId);

    public void RemoveByConnectionId(string connectionId)
    {
        var user = _userConnections.FirstOrDefault(x => x.Value == connectionId);
        if (!string.IsNullOrWhiteSpace(user.Key))
        {
            _userConnections.TryRemove(user.Key, out _);
        }
    }

    public string? GetConnectionId(string userId)
        => _userConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;

    public IReadOnlyCollection<string> GetOnlineUsers() => _userConnections.Keys.ToList().AsReadOnly();
}
