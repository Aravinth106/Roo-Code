using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RooCode.Chat.Server.Data;
using RooCode.Chat.Server.DTOs;
using RooCode.Chat.Server.Models;
using RooCode.Chat.Server.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RooCode.Chat.Server.Services;

public class AuthService(
    IUserRepository userRepository,
    IOptions<JwtSettings> jwtOptions,
    IConfiguration configuration) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existing = await userRepository.GetByEmailAsync(email);

        if (existing is not null)
        {
            throw new InvalidOperationException("Email already registered.");
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.CreateAsync(user);

        var token = GenerateToken(user);
        return new AuthResponse(token, ToDto(user));
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(email)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var token = GenerateToken(user);
        return new AuthResponse(token, ToDto(user));
    }

    public string GenerateToken(User user)
    {
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? _jwtSettings.Secret;
        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserDto ToDto(User user) => new(user.Id, user.Name, user.Email, user.CreatedAt);
}
