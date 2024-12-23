using SimpleBlog.Repositories.Users;

namespace SimpleBlog.Application.Users;

public class UsersService
{
    private readonly UsersRepository _usersRepository;
    public UsersService(UsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User> CreateUserAsync(User currentUser, CreateUserRequest userReq)
    {
        currentUser.Role.AssertPermission(Permission.UserCreate);

        var passParameters = PasswordManager.GeneratePaswordHashAndSalt(userReq.Password);

        var user = User.CreateNew(userReq.Username, userReq.Email, userReq.FirstName, passParameters.hash, passParameters.salt, userReq.LastName,RoleCode.Contributor);
        var addedUser = await _usersRepository.RegisterUserAsync(user);
        return addedUser;
    }

    public async Task<IReadOnlyList<User>> GetUsersAsyns(User currentUser)
    {
        currentUser.Role.AssertPermission(Permission.UserRead);
        
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByCredentialsAsync(string email, string password)
    {
        var user = await _usersRepository.GetUserByEmailAsync(email);

        if(user is null)
            throw new ArgumentNullException($"User with email: {email} could not be found");

        var isCorrect = PasswordManager.VerifyPasword(password, user.PasswordHash, user.PasswordSalt);

        if(isCorrect)
            return user;

        return null;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var user = await _usersRepository.GetUserByIdAsync(userId);
        return user;
    }
}
