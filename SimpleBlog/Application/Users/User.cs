namespace SimpleBlog.Application.Users;

public record class User
{
    public static User CreateNew(string username, string email, string firstName, string lastName, string passwordHash, string passwordSalt, RoleCode roleCode)
    {
        return Create(0, username, email, firstName, lastName, passwordHash, passwordSalt, roleCode);
    }

    public static User Create(int id, string username, string email, string firstName, string lastName, string passwordHash, string passwordSalt, RoleCode roleCode)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(username);
        ArgumentNullException.ThrowIfNullOrEmpty(email);
        ArgumentNullException.ThrowIfNullOrEmpty(firstName);
        ArgumentNullException.ThrowIfNullOrEmpty(lastName);

        var role = roleCode switch
        {
            RoleCode.Admin => Role.Admin,
            RoleCode.Contributor => Role.Contributor,
            _ => throw new ArgumentOutOfRangeException(nameof(roleCode), $"Not expected Role value: {roleCode}"),
        };

        return new User(id, username, email, firstName, lastName, passwordHash, passwordSalt, roleCode, role);
    }

    public int Id { get; }
    public string Username { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string PasswordHash { get; }
    public string PasswordSalt { get; }
    public RoleCode RoleCode { get; }
    public Role Role { get; }

    private User(int id, string username, string email, string firstName, string lastName, string passwordHash, string passwordSalt, RoleCode roleCode, Role role)
    {
        Id = id;
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        RoleCode = roleCode;
        Role = role;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }
}
