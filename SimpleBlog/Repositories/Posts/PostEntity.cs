using SimpleBlog.Application.Posts;

namespace SimpleBlog.Repositories.Posts;

public class PostEntity
{
    public static PostEntity From(Post post)
    {
        var entity = new PostEntity
        {
            Id = post.Id,
            Created = post.Created,
            Content = post.Content,
            Published = post.Published,
            Status = (int)post.Status,
            Title = post.Title,
            AuthorId = post.AuthorId,
            Categories = post.CategoryIds
            .Select(id => new CategoryPostRelationEntity 
            { 
                CategoryId = id, 
                PostId = post.Id,
            })
            .ToList(),
        };

        foreach (var cat in entity.Categories)
        {
            cat.Post = entity;
        }

        return entity;
    }

    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Status { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Published { get; set; }
    public List<CategoryPostRelationEntity> Categories { get; set; }

    public Post ToPost()
    {
        var categoryIds = Categories
            .Select(c => c.CategoryId)
            .ToList();

        return Post.Create(Id, AuthorId, Title, Content, (PostStatus)Status, Created, Published, categoryIds);
    }
}
