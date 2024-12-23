using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Users;

namespace SimpleBlog.WebApi.Controllers;

public class SimpleBlogController : ControllerBase
{

    private readonly UsersService _userService;
    public SimpleBlogController(UsersService userService)
    {
        _userService = userService;
    }

    internal async Task<User> GetCurrentUserAsync()
    {
        var userIdClaim = User.FindFirst(AuthService.ClaimUserId);
        if (userIdClaim is null)
        {
            throw new AuthorizationException("Authorization Error");
        }
        var userId = int.Parse(userIdClaim.Value);

        var currentUser = await _userService.GetUserByIdAsync(userId);

        if (currentUser is null)
            throw new AuthorizationException("Authorization Error");

        return currentUser;
    }
}
