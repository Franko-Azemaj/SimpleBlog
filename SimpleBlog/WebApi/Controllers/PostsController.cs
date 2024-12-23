using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Posts;
using SimpleBlog.Application.Users;
using SimpleBlog.WebApi.Posts;

namespace SimpleBlog.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : SimpleBlogController
{
    private readonly PostsService _postService;
    private readonly UsersService _userService;

    public PostsController(PostsService postService, UsersService userService) : base(userService)
    {
        _postService = postService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<PostApiModel> CreatePost(PostApiModel request)
    {
        request.Id = 0;
        var currentUser = await GetCurrentUserAsync();

        var response = await _postService.CreatePostAsync(currentUser, request.ToPost());
        return PostApiModel.From(response);
    }

    [HttpPut("{id:int}")]
    public async Task<PostApiModel> UpdatePost(int id, PostApiModel request)
    {
        var currentUser = await GetCurrentUserAsync();

        request.Id = id;
        var response = await _postService.UpdatePostAsync(currentUser, request.ToPost());
        return PostApiModel.From(response);
    }

    [HttpGet]
    public async Task<List<PostApiModel>> GetPosts([FromQuery] string? search, [FromQuery] DateTimeOffset? created)
    {
        var currentUser = await GetCurrentUserAsync();
        var response = await _postService.GetPostsAsync(currentUser, search, created);
        return response.Select(PostApiModel.From).ToList();
    }

    [HttpDelete("{id:int}")]
    public async Task DeletePost(int id)
    {
        var currentUser = await GetCurrentUserAsync();
        await _postService.DeletePostAsync(currentUser, id);
    }
}
