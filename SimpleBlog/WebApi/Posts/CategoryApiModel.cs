using SimpleBlog.Application.Posts;

namespace SimpleBlog.WebApi.Posts;

public class CategoryApiModel
{
    public static CategoryApiModel From(Category category)
    {
        return new CategoryApiModel
        {
            Id = category.Id,
            Name = category.Name,
        };
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public Category ToCategory()
    {
        return Category.Create(Id,Name);
    }
}
