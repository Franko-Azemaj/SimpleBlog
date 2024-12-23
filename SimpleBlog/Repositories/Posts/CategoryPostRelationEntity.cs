using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlog.Repositories.Posts;

[PrimaryKey(nameof(CategoryId), nameof(PostId))]
public class CategoryPostRelationEntity
{
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public CategoryEntity Category { get; set; }

    public int PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public PostEntity Post { get; set; }
}