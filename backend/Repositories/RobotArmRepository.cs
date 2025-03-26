namespace RoboticArmSim.Repositories;

using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Data;
using RoboticArmSim.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


public class RobotArmRepository : IRobotArmRepository
{
    private readonly ApplicationDbContext _context;

    public RobotArmRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RobotArm?> GetArmByIdAsync(int armId)
    {
        return await _context.RobotArms.FindAsync(armId);
    }

    public async Task<List<RobotArm>> GetAllArmsAsync()
    {
        return await _context.RobotArms.ToListAsync();
    }

    public async Task AddRobotArmAsync(RobotArm robotArm)
    {
        await _context.RobotArms.AddAsync(robotArm);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRobotArmAsync(RobotArm robotArm)
    {
        _context.RobotArms.Update(robotArm);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArmAsync(int armId)
    {
        var robotArm = await _context.RobotArms.FindAsync(armId);
        if (robotArm != null)
        {
            _context.RobotArms.Remove(robotArm);
            await _context.SaveChangesAsync();
        }
    }
}