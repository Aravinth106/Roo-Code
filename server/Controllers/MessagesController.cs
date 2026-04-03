using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RooCode.Chat.Server.DTOs;
using RooCode.Chat.Server.Extensions;
using RooCode.Chat.Server.Models;
using RooCode.Chat.Server.Repositories;

namespace RooCode.Chat.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MessagesController(IMessageRepository messageRepository) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<MessageDto>>> GetConversation(string userId)
    {
        var currentUserId = User.GetUserId();
        var messages = await messageRepository.GetConversationAsync(currentUserId, userId);

        return Ok(messages.Select(m => new MessageDto(m.Id, m.SenderId, m.ReceiverId, m.Content, m.Timestamp)).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> Create([FromBody] SendMessageRequest request)
    {
        var currentUserId = User.GetUserId();

        var message = new Message
        {
            SenderId = currentUserId,
            ReceiverId = request.ReceiverId,
            Content = request.Content.Trim(),
            Timestamp = DateTime.UtcNow
        };

        var created = await messageRepository.CreateAsync(message);
        return Ok(new MessageDto(created.Id, created.SenderId, created.ReceiverId, created.Content, created.Timestamp));
    }
}
