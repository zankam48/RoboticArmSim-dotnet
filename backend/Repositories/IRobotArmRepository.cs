namespace RoboticArmSim.Repositories;
using RoboticArmSim.Models;


public interface IRobotArmRepository
{
    Task<RobotArm?> GetArmByIdAsync(int armId);
    Task<List<RobotArm>> GetAllArmsAsync();
    Task AddRobotArmAsync(RobotArm robotArm);
    Task UpdateRobotArmAsync(RobotArm robotArm);
    Task DeleteArmAsync(int armId);
}
