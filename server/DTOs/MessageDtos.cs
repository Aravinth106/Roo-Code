namespace RooCode.Chat.Server.DTOs;

public record SendMessageRequest(string ReceiverId, string Content);
public record MessageDto(string Id, string SenderId, string ReceiverId, string Content, DateTime Timestamp);
