using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RoboticArmSim.Models;
using RoboticArmSim.DTOs;
using RoboticArmSim.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace RoboticArmSim.Services
{
    public class RobotArmService
    {
        private readonly ILogger<RobotArmService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<RoboticArmHub> _hubContext;
        private readonly IMapper _mapper;

        public RobotArmService(ILogger<RobotArmService> logger, IHubContext<RoboticArmHub> hubContext, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task<RobotArmDTO> CreateRobotArmAsync(RobotArmDTO robotArmDTO)
        {
            var robotArm = new RobotArm
            {
                PositionX = robotArmDTO.PositionX,
                PositionY = robotArmDTO.PositionY,
                PositionZ = robotArmDTO.PositionZ,
                Rotation = robotArmDTO.Rotation
            };

            robotArm.SetJointAngles(robotArmDTO.JointAngles ?? new List<float> { 0, 0, 0, 0, 0, 0 });

            _context.RobotArms.Add(robotArm);
            await _context.SaveChangesAsync();
            return _mapper.Map<RobotArmDTO>(robotArm);
        }

        public async Task<RobotArmDTO?> MoveArmAsync(MovementCommand command)
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

            List<float> jointAngles = robotArm.GetJointAngles();

            int jointIndex = int.TryParse(command.Joint.Replace("Joint", ""), out int index) ? index - 1 : -1;
            if (jointIndex < 0 || jointIndex >= jointAngles.Count)
            {
                _logger.LogWarning($"Invalid joint index: {jointIndex}. Must be between 0 and {jointAngles.Count - 1}");
                return null;
            }

            jointAngles[jointIndex] = command.Angle;
            robotArm.SetJointAngles(jointAngles);  

            await _context.SaveChangesAsync();

            var robotArmDto = new RobotArmDTO
            {
                PositionX = robotArm.PositionX,
                PositionY = robotArm.PositionY,
                PositionZ = robotArm.PositionZ,
                Rotation = robotArm.Rotation,
                JointAngles = jointAngles
            };

            await _hubContext.Clients.All.SendAsync("ReceiveArmUpdate", robotArmDto);

            _logger.LogInformation("Movement successfully processed and broadcasted");
            return robotArmDto;
        }

        public async Task<RobotArmDTO?> GetArmStateAsync(int armId)
        {
            var robotArm = await _context.RobotArms.FindAsync(armId);
            if (robotArm == null) return null;

            var robotArmDto = new RobotArmDTO
            {
                PositionX = robotArm.PositionX,
                PositionY = robotArm.PositionY,
                PositionZ = robotArm.PositionZ,
                Rotation = robotArm.Rotation
            };

            robotArmDto.SetJointAnglesFromJson(robotArm.JointAngles);

            return robotArmDto;
        }


        public async Task<List<RobotArmDTO>> GetAllArmsAsync()
        {
            var arms = await _context.RobotArms.ToListAsync();
            return _mapper.Map<List<RobotArmDTO>>(arms);
        }

        public async Task ResetArmAsync(int armId)
        {
            var robotArm = await _context.RobotArms.FindAsync(armId);
            if (robotArm != null)
            {
                robotArm.PositionX = 0;
                robotArm.PositionY = 0;
                robotArm.PositionZ = 0;
                robotArm.Rotation = 0;
                
                robotArm.SetJointAngles(new List<float> { 0, 0, 0, 0, 0, 0 });

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Robot arm {armId} reset to default state.");
            }
        }
    }
}
