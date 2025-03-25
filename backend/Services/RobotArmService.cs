using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RoboticArmSim.Models;
using RoboticArmSim.DTOs;
using RoboticArmSim.Data;
using Microsoft.EntityFrameworkCore;

namespace RoboticArmSim.Services;

public class RobotArmService
{
    private readonly ILogger<RobotArmService> _logger;
    private readonly RobotArm _robotArm;
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<RoboticArmHub> _hubContext;

    public RobotArmService(ILogger<RobotArmService> logger, IHubContext<RoboticArmHub> hubContext, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<RobotArmDTO> MoveArmAsync(MovementCommand command)
    {
        _logger.LogInformation($"Processing movement command: ArmId={command.ArmId}, Joint={command.Joint}, Angle={command.Angle}");
        
        var robotArm = await _context.RobotArms.FindAsync(command.ArmId);
        if (robotArm == null)
        {
            _logger.LogWarning($"RobotArm with ID {command.ArmId} not found.");
            return null;
        }
        if (command.Angle < 0 || command.Angle > 180)
        {
            _logger.LogWarning("Invalid movement: Angle must be between 0 and 180.");
            return null;
        }

        int jointIndex = int.TryParse(command.Joint.Replace("Joint", ""), out int index) ? index - 1 : -1;
        if (jointIndex <= 0 && jointIndex > robotArm.JointAngles.Count)
        {
            _logger.LogWarning($"Invalid joint index: {jointIndex}. Must be between 0 and {robotArm.JointAngles.Count -1}");
            return null;
        }

        robotArm.JointAngles[jointIndex] = command.Angle;
        await _context.SaveChangesAsync();

        var robotArmDto = new RobotArmDTO
        {
            PositionX = robotArm.PositionX,
            PositionY = robotArm.PositionY,
            PositionZ = robotArm.PositionZ,
            Rotation = robotArm.Rotation,
            JointAngles = robotArm.JointAngles
        };

        await _hubContext.Clients.All.SendAsync("ReceiveArmUpdate", robotArmDto);

        _logger.LogInformation("Movement successfully processed and broadcasted");
        return robotArmDto;
    }

    public async Task<RobotArm?> GetArmStateAsync(int ArmId)
    {
        return await _context.RobotArms.FindAsync(ArmId);
    }

    public async Task<List<RobotArm>> GetAllArmsAsync()
    {
        return await _context.RobotArms.ToListAsync();
    }

    public async Task ResetArmAsync(int ArmId)
    {
        var robotArm = await _context.RobotArms.FindAsync(ArmId);
        if (robotArm != null)
        {
            robotArm.PositionX = 0;
            robotArm.PositionY = 0;
            robotArm.PositionZ = 0;
            robotArm.Rotation = 0;
            robotArm.JointAngles = new List<float> { 0, 0, 0, 0, 0, 0 };
            // robotArm.JointAngles.Clear();
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Robot arm {ArmId} reset to default state.");
        }
    }
}




  

