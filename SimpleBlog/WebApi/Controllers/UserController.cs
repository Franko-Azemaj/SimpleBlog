using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Users;
using SimpleBlog.WebApi.Users;

namespace SimpleBlog.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : SimpleBlogController
{
    private readonly UsersService _userService;
    public UserController(UsersService userService) : base(userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<UserApiModel> CreateUser(CreateUserRequestApiModel request)
    {
        var currentUser = await GetCurrentUserAsync();

        var response = await _userService.CreateUserAsync(currentUser, request.ToCreateUserRequest());
        return UserApiModel.From(response);
    }
}
