using FluentMigrator;
using SimpleBlog.Application.Posts;
using SimpleBlog.Application.Users;
using SimpleBlog.Repositories.Posts;
using SimpleBlog.Repositories.Users;

namespace SimpleBlog.Repositories.Migrations;

[Migration(0)]
public class InitialMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        this.Execute.Sql("PRAGMA foreign_keys = ON");

        Create.Table("Users")
               .WithColumn("Id").AsInt32().PrimaryKey().Identity()
               .WithColumn("Username").AsString()
               .WithColumn("Email").AsString()
               .WithColumn("FirstName").AsString()
               .WithColumn("LastName").AsString()
               .WithColumn("PasswordHash").AsString()
               .WithColumn("PasswordSalt").AsString()
               .WithColumn("RoleCode").AsInt32();

        // Generating Admin User
        var hashAndSalt = PasswordManager.GeneratePaswordHashAndSalt("Admin123");
        var userEntity = new UserEntity
        {
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@mail.com",
            Username = "admin",
            RoleCode = (int)RoleCode.Admin,
            PasswordHash = hashAndSalt.hash,
            PasswordSalt = hashAndSalt.salt
        };
        Insert.IntoTable("Users").Row(userEntity);

        Create.Table("Categories")
               .WithColumn("Id").AsInt32().PrimaryKey().Identity()
               .WithColumn("Name").AsString();

        Create.Table("Posts")
               .WithColumn("Id").AsInt32().PrimaryKey().Identity()
               .WithColumn("AuthorId").AsString()
               .WithColumn("Title").AsString()
               .WithColumn("Content").AsString()
               .WithColumn("Status").AsInt32()
               .WithColumn("Created").AsString()
               .WithColumn("Published").AsString();

        this.Execute.Sql("""
                CREATE TABLE CategoryPostRelations (
                CategoryId INTEGER NOT NULL, 
                PostId INTEGER NOT NULL, 
                PRIMARY KEY (CategoryId, PostId),
                FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
                FOREIGN KEY (PostId) REFERENCES Posts(Id)
            );
            """);

        // Generating some Categories as exapmle for the program to have at the start
        var categoryEntity = new
        {
            Name = "Category1"
        };
        var categoryEntity2 = new
        {
            Name = "Category2"
        };
        var categoryEntity3 = new
        {
            Name = "Category3"
        };

        Insert.IntoTable("Categories").Row(categoryEntity);
        Insert.IntoTable("Categories").Row(categoryEntity2);
        Insert.IntoTable("Categories").Row(categoryEntity3);
    }
}