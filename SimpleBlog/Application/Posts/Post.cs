using SimpleBlog.Application.Users;

namespace SimpleBlog.Application.Posts;

public class Post
{
    public static Post Create(int id, int authorId, string title, string content, PostStatus status, DateTimeOffset ceatedDate, DateTimeOffset publishDate, IReadOnlyList<int> categoryIds)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(title);
        ArgumentNullException.ThrowIfNullOrEmpty(content);

        return new Post(id, authorId, title, content, status, ceatedDate, publishDate, categoryIds);
    }

    public int Id { get; }
    public int AuthorId { get; }
    public string Title{ get; }
    public string Content { get; }
    public PostStatus Status { get; }
    public DateTimeOffset Created{ get; }
    public DateTimeOffset Published { get; }
    public IReadOnlyList<int> CategoryIds { get; }

    private Post(int id,int authorId, string title, string content, PostStatus status, DateTimeOffset ceatedDate, DateTimeOffset publishDate, IReadOnlyList<int> categoryIds)
    {
        Id = id;
        Title = title;
        Content = content;
        Status = status;
        Created = ceatedDate;
        Published= publishDate;
        CategoryIds = categoryIds;
        AuthorId = authorId;
    }
}
