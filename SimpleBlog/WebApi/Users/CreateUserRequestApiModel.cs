using SimpleBlog.Application.Users;

namespace SimpleBlog.WebApi.Users;

public class CreateUserRequestApiModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }

    public CreateUserRequest ToCreateUserRequest()
    {
        return CreateUserRequest.Create(Username, Email, FirstName, LastName,Password);  
    }
}
