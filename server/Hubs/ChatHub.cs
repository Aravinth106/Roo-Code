using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RooCode.Chat.Server.DTOs;
using RooCode.Chat.Server.Extensions;
using RooCode.Chat.Server.Models;
using RooCode.Chat.Server.Repositories;
using RooCode.Chat.Server.Services;

namespace RooCode.Chat.Server.Hubs;

[Authorize]
public class ChatHub(
    IOnlineUserTracker onlineUserTracker,
    IMessageRepository messageRepository) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.GetUserId();
        if (!string.IsNullOrWhiteSpace(userId))
        {
            onlineUserTracker.AddOrUpdate(userId, Context.ConnectionId);
            await BroadcastOnlineUsers();
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        onlineUserTracker.RemoveByConnectionId(Context.ConnectionId);
        await BroadcastOnlineUsers();
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinUserRoom(string userId)
    {
        onlineUserTracker.AddOrUpdate(userId, Context.ConnectionId);
        await BroadcastOnlineUsers();
    }

    public async Task SendMessage(string receiverId, string content)
    {
        var senderId = Context.User?.GetUserId()
            ?? throw new HubException("Unauthorized");

        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content.Trim(),
            Timestamp = DateTime.UtcNow
        };

        var saved = await messageRepository.CreateAsync(message);
        var payload = new MessageDto(saved.Id, saved.SenderId, saved.ReceiverId, saved.Content, saved.Timestamp);

        await Clients.Caller.SendAsync("ReceiveMessage", payload);

        var receiverConnectionId = onlineUserTracker.GetConnectionId(receiverId);
        if (!string.IsNullOrWhiteSpace(receiverConnectionId))
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", payload);
        }
    }

    public async Task BroadcastOnlineUsers()
    {
        await Clients.All.SendAsync("OnlineUsers", onlineUserTracker.GetOnlineUsers());
    }
}
