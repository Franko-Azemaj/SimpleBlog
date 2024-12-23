namespace SimpleBlog.Application.Users;
public class CreateUserRequest
{
    public static CreateUserRequest Create(string username, string email, string firstName, string lastName, string password)
    {
        return new CreateUserRequest(username,email,firstName,lastName,password);
    }

    public string Username { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Password { get; }

    private CreateUserRequest(string username, string email, string firstName, string lastName, string password)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
    }
}
