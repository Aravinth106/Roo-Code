using RooCode.Chat.Server.DTOs;
using RooCode.Chat.Server.Models;

namespace RooCode.Chat.Server.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    string GenerateToken(User user);
}
