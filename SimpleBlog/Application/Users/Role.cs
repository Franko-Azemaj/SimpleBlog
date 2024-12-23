namespace SimpleBlog.Application.Users;

public class Role
{
    public static Role Admin { get; } = new Role(RoleCode.Admin, "Administrator", new HashSet<Permission>() { 
        Permission.UserCreate, 
        Permission.UserUpdate,
        Permission.UserDelete,
        Permission.UserRead,

        Permission.CategoryRead,
        Permission.CategoryUpdate,
        Permission.CategoryDelete,
        Permission.CategoryCreate,

        Permission.PostCreate,
        Permission.PostUpdate,
        Permission.PostDelete,
        Permission.PostRead
    });

    public static Role Contributor { get; } = new Role(RoleCode.Contributor, "Contributor", new HashSet<Permission>() {
        Permission.CategoryRead,
        Permission.CategoryUpdate,
        Permission.CategoryDelete,
        Permission.CategoryCreate,

        Permission.PostCreate,
        Permission.PostUpdate,
        Permission.PostDelete,
        Permission.PostRead
    });

    public RoleCode Code { get; }
    public string Name { get; }
    public HashSet<Permission> Permissions;

    private Role(RoleCode code, string name, HashSet<Permission> permissions)
    {
        Code = code;
        Name = name;
        Permissions = permissions;
    }

    public bool HasPermission(Permission permission)
    {
        return Permissions.Contains(permission);
    }

    public void AssertPermission(Permission permission)
    {
        if (!HasPermission(permission))
            throw new AuthorizationException($"Permission required:{permission}");
    }
}
