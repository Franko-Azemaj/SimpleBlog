using Microsoft.EntityFrameworkCore;
using SimpleBlog.Application.Posts;
using SimpleBlog.Repositories.DatabaseContext;

namespace SimpleBlog.Repositories.Posts;

public class PostsRepository
{
    private readonly ApplicationDbContext _context;
    public PostsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        var catIds = await _context.Categories.Select(cat => cat.Id).ToListAsync();
        var invalidIds = post.CategoryIds.Except(catIds).ToList();

        if (invalidIds.Any())
        {
            throw new ArgumentException($"Category with id {invalidIds[0]} does not exist");
        }

        var postEntity = PostEntity.From(post);
        _context.Posts.Add(postEntity);
        await _context.SaveChangesAsync();
        return postEntity.ToPost();
    }

    public async Task<Post> UpdatePostAsync(Post post)
    {
        var postEntity = await _context.Posts
            .Include(c => c.Categories)
            .FirstOrDefaultAsync(p => p.Id == post.Id);

        if (postEntity is null)
            throw new ArgumentException("Post object does not exist");

        if (post.AuthorId != postEntity.AuthorId)
            throw new ArgumentException("AuthorId does not match!");

        postEntity.Status = (int)post.Status;
        postEntity.Title = post.Title;
        postEntity.Content = post.Content;

        // Deleted Categories
        var deletedCategoreis = postEntity.Categories
            .Where(c => !post.CategoryIds.Contains(c.CategoryId))
            .ToList();

        if (deletedCategoreis.Any())
            _context.CategoryPostRelations.RemoveRange(deletedCategoreis);

        // Added Categories
        var existingCategoryIds = postEntity.Categories.Select(x => x.CategoryId).ToList();
        var addedCategoryIds = post.CategoryIds.Where(c => !existingCategoryIds.Contains(c)).ToList();
        var addedCategories = addedCategoryIds
            .Select(id => new CategoryPostRelationEntity
            {
                CategoryId = id,
                PostId = post.Id,
            })
            .ToList();

        if (addedCategoryIds.Any())
            _context.CategoryPostRelations.AddRange(addedCategories);

        await _context.SaveChangesAsync();
        return postEntity.ToPost();
    }

    public async Task DeletePostAsync(int id)
    {
        var postEntity = await _context.Posts
            .Include(c => c.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (postEntity is null)
            return;

        _context.Posts.Remove(postEntity);
        _context.CategoryPostRelations.RemoveRange(postEntity.Categories);
        await _context.SaveChangesAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        var postEntity = await _context.Posts.Include(c => c.Categories).FirstOrDefaultAsync(p => p.Id == id);
        return postEntity?.ToPost();
    }

    public async Task<IReadOnlyList<Post>> GetPostsAsync(
        string? search, DateTimeOffset? date)
    {

        var query = _context.Posts
            .Include(c => c.Categories)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));
        }

        if (date.HasValue)
        {
            query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));
        }

        var postEntities = await query.ToListAsync();

        var posts = postEntities.Select(p => p.ToPost()).ToList();
        return posts;
    }

    //Category CRUD
    public async Task<Category> CreateCategoryAsync(Category category)
    {
        var categoryEntity = CategoryEntity.From(category);

        _context.Categories.Add(categoryEntity);
        await _context.SaveChangesAsync();

        return categoryEntity.ToCategory();
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        var categoryEntity = await _context.Categories.FirstOrDefaultAsync(p => p.Id == category.Id);

        if (categoryEntity is null)
            throw new ArgumentException("Category object does not exist");

        categoryEntity.Name = category.Name;

        _context.Categories.Update(categoryEntity);
        await _context.SaveChangesAsync();

        return categoryEntity.ToCategory();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var categoryEntity = await _context.Categories
            .Include(c => c.Posts.Take(1))
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoryEntity.Posts.Any())
            throw new ArgumentException("Can not delete categories which contain posts");

        if (categoryEntity is null)
            return;

        _context.Categories.Remove(categoryEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var categoryEntity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return categoryEntity?.ToCategory();
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
    {
        var categoriesEntity = await _context.Categories.ToListAsync();
        var categories = categoriesEntity.Select(c => c.ToCategory()).ToList();
        return categories;
    }
}