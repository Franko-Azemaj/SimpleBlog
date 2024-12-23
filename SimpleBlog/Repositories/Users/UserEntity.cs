using SimpleBlog.Application.Users;

namespace SimpleBlog.Repositories.Users;

public class UserEntity
{
    public static UserEntity From(User user)
    {
        return new UserEntity
        {
            Email = user.Email,
            FirstName = user.FirstName,
            Id = user.Id,
            LastName = user.LastName,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            RoleCode = (int)user.RoleCode,
            Username = user.Username
        };
    }

    public int Id { get; set; }
    public string Username { get; set; }
    public string Email{ get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public int RoleCode { get; set; }

    public User ToUser()
    {
        return User.Create(Id, Username, Email, FirstName, LastName,PasswordHash,PasswordSalt, (RoleCode)RoleCode);
    }
}
