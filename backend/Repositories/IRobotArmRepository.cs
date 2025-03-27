namespace RoboticArmSim.Repositories;
using RoboticArmSim.Models;


public interface IRobotArmRepository
{
    Task<RobotArm?> GetArmByIdAsync(Guid armId);
    Task<List<RobotArm>> GetAllArmsAsync();
    Task AddRobotArmAsync(RobotArm robotArm);
    Task UpdateRobotArmAsync(RobotArm robotArm);
    Task DeleteArmAsync(Guid armId);
}
