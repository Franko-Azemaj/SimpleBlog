using Microsoft.Extensions.Hosting;
using SimpleBlog.Application.Users;
using SimpleBlog.Repositories.Posts;

namespace SimpleBlog.Application.Posts;

public class PostsService
{
    private readonly PostsRepository _postRepository;
    public PostsService(PostsRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Post> CreatePostAsync(User currentUser, Post post)
    {
        currentUser.Role.AssertPermission(Permission.PostCreate);
        var addedPost = await _postRepository.CreatePostAsync(post);
        return addedPost;
    }

    public async Task<Post> UpdatePostAsync(User currentUser, Post post)
    {
        currentUser.Role.AssertPermission(Permission.PostUpdate);

        if (currentUser.Id != post.AuthorId) 
            throw new UnauthorizedAccessException();

        var updatedPost = await _postRepository.UpdatePostAsync(post);
        return updatedPost;
    }

    public async Task DeletePostAsync(User currentUser,int id)
    {
        currentUser.Role.AssertPermission(Permission.PostDelete);

        var post = await _postRepository.GetPostByIdAsync(id);
        if(currentUser.Id != post.AuthorId)
            throw new UnauthorizedAccessException();

        await _postRepository.DeletePostAsync(id);
    }

    public async Task<IReadOnlyList<Post>> GetPostsAsync(User currentUser, string? search , DateTimeOffset? date)
    {
        currentUser.Role.AssertPermission(Permission.PostCreate);
        var Posts = await _postRepository.GetPostsAsync(search, date);
        return Posts;
    }

    //Category Service
    public async Task<Category> CreateCategoryAsync(User currentUser, Category category)
    {
        currentUser.Role.AssertPermission(Permission.CategoryCreate);
        var createdRepository = await _postRepository.CreateCategoryAsync(category);
        return createdRepository;
    }

    public async Task<Category> UpdateCategoryAsync(User currentUser, Category category)
    {
        currentUser.Role.AssertPermission(Permission.CategoryUpdate);
        var updatedCategory = await _postRepository.UpdateCategoryAsync(category);
        return updatedCategory;
    }

    public async Task DeleteCategoryAsync(User currentUser, int id)
    {
        currentUser.Role.AssertPermission(Permission.CategoryDelete);
        await _postRepository.DeleteCategoryAsync(id);
    }

    public async Task<Category> GetCategoryByIdAsync(User currentUser, int id)
    {
        currentUser.Role.AssertPermission(Permission.CategoryRead);
        var category = await _postRepository.GetCategoryByIdAsync(id);
       return category;
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsyns(User currentUser)
    {
        currentUser.Role.AssertPermission(Permission.CategoryRead);
        var categories = await _postRepository.GetCategoriesAsync();
        return categories;
    }
}
