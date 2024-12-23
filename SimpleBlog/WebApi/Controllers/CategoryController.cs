using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Posts;
using SimpleBlog.Application.Users;
using SimpleBlog.WebApi.Posts;

namespace SimpleBlog.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : SimpleBlogController
{
    private readonly PostsService _postsService;
    private readonly UsersService _userService;
    public CategoryController(PostsService postsService, UsersService userService) : base(userService)
    {
        _postsService = postsService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<CategoryApiModel> CreateCategory(CategoryApiModel request)
    {
        request.Id = 0;
        var currentUser = await GetCurrentUserAsync();

        var response = await _postsService.CreateCategoryAsync(currentUser, request.ToCategory());
        return CategoryApiModel.From(response);
    }

    [HttpPut("{id:int}")]
    public async Task<CategoryApiModel> UpdateCategory(int id,CategoryApiModel request)
    {
        var currentUser = await GetCurrentUserAsync();

        request.Id = id;
        var response = await _postsService.UpdateCategoryAsync(currentUser, request.ToCategory());
        return CategoryApiModel.From(response);
    }

    [HttpGet("{id:int}")]
    public async Task<CategoryApiModel> GetCategoryById(int id)
    {
        var currentUser = await GetCurrentUserAsync();
        var response = await _postsService.GetCategoryByIdAsync(currentUser, id);
        return CategoryApiModel.From(response);
    }

    [HttpGet]
    public async Task<IReadOnlyList<CategoryApiModel>> GetCategories()
    {
        var currentUser = await GetCurrentUserAsync();
        var categories = await _postsService.GetCategoriesAsyns(currentUser);
        return categories.Select(CategoryApiModel.From).ToList();
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteCategory(int id)
    {
        var currentUser = await GetCurrentUserAsync();
        await _postsService.DeleteCategoryAsync(currentUser, id);
    }
}