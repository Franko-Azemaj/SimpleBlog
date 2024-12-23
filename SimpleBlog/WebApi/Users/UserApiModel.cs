using SimpleBlog.Application.Users;
namespace SimpleBlog.WebApi.Users;

public class UserApiModel
{
    public static UserApiModel From(User user)
    {
        return new UserApiModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            RoleCode = user.RoleCode,
            Username = user.Username
        };
    }

    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public RoleCode RoleCode { get; set; }
    public Role Role { get; set; }
}