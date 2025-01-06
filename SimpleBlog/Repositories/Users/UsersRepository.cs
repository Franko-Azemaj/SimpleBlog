using Microsoft.EntityFrameworkCore;
using SimpleBlog.Application.Users;
using SimpleBlog.Repositories.DatabaseContext;
namespace SimpleBlog.Repositories.Users;

public class UsersRepository
{
    private readonly ApplicationDbContext _context;
    public UsersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        var userEntity = UserEntity.From(user);

        _context.Users.Add(userEntity);
        _context.SaveChanges();

        return userEntity.ToUser();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        return userEntity?.ToUser();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        return userEntity?.ToUser();
    }
}
