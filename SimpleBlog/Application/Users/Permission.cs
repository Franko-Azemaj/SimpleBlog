namespace SimpleBlog.Application.Users;

public enum Permission
{
    // Users
    UserRead = 100,
    UserCreate,
    UserUpdate,
    UserDelete,

    // Posts
    PostRead = 1000,
    PostCreate,
    PostUpdate,
    PostDelete,

    CategoryRead = 1100,
    CategoryCreate,
    CategoryUpdate,
    CategoryDelete,
}
