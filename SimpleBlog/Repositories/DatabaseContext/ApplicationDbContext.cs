using Microsoft.EntityFrameworkCore;
using SimpleBlog.Application.Posts;
using SimpleBlog.Repositories.Posts;
using SimpleBlog.Repositories.Users;

namespace SimpleBlog.Repositories.DatabaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<CategoryPostRelationEntity> CategoryPostRelations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostEntity>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
    }
}