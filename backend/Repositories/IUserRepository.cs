namespace RoboticArmSim.Repositories;

using RoboticArmSim.Models;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<bool> IsUniqueUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task<List<User>> GetAllAsync();
    Task SaveChangesAsync();
}