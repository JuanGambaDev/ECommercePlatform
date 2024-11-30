using UserService.Interfaces;
using UserService.Data;
using UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repositories;

public class UserRepository : IUserRepository 
{
    private readonly UserServiceContext _dbContext;

    public UserRepository(UserServiceContext dbContext)
    {
        _dbContext = dbContext;
    }    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users.Where (x => x.UserId == id).FirstOrDefaultAsync();
    }

    public async  Task<bool> CreateAsync(User user)
    {
        _dbContext.Users.Add(user);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user == null) return false;

        _dbContext.Users.Remove(user);
        return await _dbContext.SaveChangesAsync() > 0;
    }

}