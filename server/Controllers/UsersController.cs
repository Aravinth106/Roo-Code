using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RooCode.Chat.Server.DTOs;
using RooCode.Chat.Server.Extensions;
using RooCode.Chat.Server.Repositories;

namespace RooCode.Chat.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var currentUserId = User.GetUserId();
        var users = await userRepository.GetAllExceptAsync(currentUserId);

        return Ok(users.Select(user => new UserDto(user.Id, user.Name, user.Email, user.CreatedAt)).ToList());
    }
}
