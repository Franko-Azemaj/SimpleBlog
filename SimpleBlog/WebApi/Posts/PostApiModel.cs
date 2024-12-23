using SimpleBlog.Application.Posts;

namespace SimpleBlog.WebApi.Posts;

public class PostApiModel
{
    public static PostApiModel From(Post post)
    {
        return new PostApiModel
        {
            Published = post.Published,
            Created = post.Created,
            Content = post.Content, 
            Id = post.Id,
            Status = post.Status,   
            Title = post.Title ,
            AuthorId = post.AuthorId ,
            CategoryIds = post.CategoryIds,
        };
    }

    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PostStatus Status { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Published { get; set; }
    public IReadOnlyList<int> CategoryIds { get; set; }

    public Post ToPost()
    {
        return Post.Create(Id, AuthorId,Title, Content,Status,Created,Published, CategoryIds);
    }
}