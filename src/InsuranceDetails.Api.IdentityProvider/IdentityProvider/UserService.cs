using InsuranceDetails.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDetails.Api.IdentityProvider;

public interface IUserService
{
    Task<User?> GetUserAsync(string email, string password);

    Task<bool> AddUserAsync(string name, string email, string password);
}

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserAsync(string email, string password)
    {
        var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            return null; // User not found
        }
        
        var isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.Salt);
        
        return isValid 
            ? user 
            : null;
    }
    
    public async Task<bool> AddUserAsync(string name, string email, string password)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser is not null)
        {
            return false; // User already exists
        }
        
        var salt = new byte[16];
        new Random().NextBytes(salt);

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = PasswordHasher.HashPassword(password, salt),
            Salt = salt
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
