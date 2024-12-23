using SimpleBlog.Application.Posts;

namespace SimpleBlog.Repositories.Posts;

public class CategoryEntity
{
    public static CategoryEntity From(Category category)
    {
        return new CategoryEntity
        {
            Id = category.Id,
            Name = category.Name,
        };
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public List<CategoryPostRelationEntity> Posts { get; set; }

    public Category ToCategory()
    {
        return Category.Create(Id,Name);
    }
}
