using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RoboticArmSim.Models;
using RoboticArmSim.DTOs;

namespace RoboticArmSim.Services;

public class RobotArmService
{
    private readonly ILogger<RobotArmService> _logger;
    private readonly RobotArm _robotArm;
    private readonly IHubContext<RoboticArmHub> _hubContext;

    public RobotArmService(ILogger<RobotArmService> logger, IHubContext<RoboticArmHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task<RobotArmDTO> MoveArmAsync(MovementCommand command)
    {
        _logger.LogInformation($"Processing movement command: ArmId={command.ArmId}, Joint={command.Joint}, Angle={command.Angle}");
        
        if (command.Angle < 0)
        {
            _logger.LogWarning("Invalid movement: Angle must be positive.");
            return null;
        }

        var robotArm = new RobotArm
        {
            Id = command.ArmId,
            PositionX = 0,
            PositionY = 0,
            PositionZ = 0,
            Rotation = 0,
            JointAngles = new List<float> { 0, 0, 0, 0 }
        };

        int jointIndex = int.TryParse(command.Joint.Replace("Joint", ""), out int index) ? index - 1 : -1;
        if (jointIndex >= 0 && jointIndex < robotArm.JointAngles.Count)
        {
            robotArm.JointAngles[jointIndex] = command.Angle;
        }
        else
        {
            _logger.LogWarning("Invalid joint index.");
            return null;
        }

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

    public RobotArm GetArmState()
    {
        return _robotArm;
    }

    public RobotArm GetArmStats()
    {
        return _robotArm;
    }

    public void UpdateArm(RobotArm updatedArm)
    {
        _robotArm.PositionX = updatedArm.PositionX;
        _robotArm.PositionY = updatedArm.PositionY;
        _robotArm.PositionZ = updatedArm.PositionZ;
        _robotArm.Rotation = updatedArm.Rotation;
        _robotArm.JointAngles = new List<float>(updatedArm.JointAngles);
        _logger.LogInformation("Robot arm updated successfully.");
    }

    public void ResetArm()
    {
        _robotArm.PositionX = 0;
        _robotArm.PositionY = 0;
        _robotArm.PositionZ = 0;
        _robotArm.Rotation = 0;
        _robotArm.JointAngles = new List<float> { 0, 0, 0, 0 };
        _logger.LogInformation("Robot arm reset to default state.");
    }
}




  

