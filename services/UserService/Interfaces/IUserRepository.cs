using UserService.Models;

namespace UserService.Interfaces;

public interface IUserRepository {
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> CreateAsync(User user);
    Task<bool> DeleteAsync(int id);

}