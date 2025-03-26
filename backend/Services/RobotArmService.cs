using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RoboticArmSim.Models;
using RoboticArmSim.DTOs;
using RoboticArmSim.Data;
using RoboticArmSim.Validators;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using System.Net;
using RoboticArmSim.Repositories;

namespace RoboticArmSim.Services
{
    public class RobotArmService
    {
        private readonly ILogger<RobotArmService> _logger;
        // private readonly ApplicationDbContext _context;
        private readonly IRobotArmRepository _robotArmRepository;
        private readonly IHubContext<RoboticArmHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRobotArmDTO> _validator;

        public RobotArmService(ILogger<RobotArmService> logger, IHubContext<RoboticArmHub> hubContext, 
                                ApplicationDbContext context, IMapper mapper, IValidator<CreateRobotArmDTO> validator, IRobotArmRepository robotArmRepository)
        {
            _logger = logger;
            // _context = context;
            _robotArmRepository = robotArmRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ApiResponse<RobotArmDTO>> CreateRobotArmAsync(CreateRobotArmDTO createDto)
        {
            var validationResult = await _validator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<RobotArmDTO>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var robotArm = new RobotArm
            {
                PositionX = createDto.PositionX,
                PositionY = createDto.PositionY,
                PositionZ = createDto.PositionZ,
                Rotation = createDto.Rotation
            };

            robotArm.SetJointAngles(createDto.JointAngles ?? new List<float> { 0, 0, 0, 0, 0, 0 });

            // _context.RobotArms.Add(robotArm);
            // await _context.SaveChangesAsync();
            await _robotArmRepository.AddRobotArmAsync(robotArm);
            return new ApiResponse<RobotArmDTO>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data = _mapper.Map<RobotArmDTO>(robotArm)
            };
        }

        public async Task<RobotArmDTO?> MoveArmAsync(MovementCommand command)
        {
            _logger.LogInformation($"Processing movement command: ArmId={command.ArmId}, Joint={command.Joint}, Angle={command.Angle}");
            
            // var robotArm = await _context.RobotArms.FindAsync(command.ArmId);
            var robotArm = await _robotArmRepository.GetArmByIdAsync(command.ArmId);
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

            int jointIndex = command.Joint;
            if (jointIndex < 0 || jointIndex >= jointAngles.Count)
            {
                _logger.LogWarning($"Invalid joint index: {jointIndex}. Must be between 0 and {jointAngles.Count - 1}");
                return null;
            }

            jointAngles[jointIndex] = command.Angle;
            robotArm.SetJointAngles(jointAngles);  

            // await _context.SaveChangesAsync();
            await _robotArmRepository.UpdateRobotArmAsync(robotArm);

            var robotArmDto = _mapper.Map<RobotArmDTO>(robotArm);

            await _hubContext.Clients.All.SendAsync("ReceiveArmUpdate", robotArmDto);

            return robotArmDto;
        }

        public async Task<RobotArmDTO?> GetArmByIdAsync(int armId)
        {
            // var robotArm = await _context.RobotArms.FindAsync(armId);
            var robotArm = await _robotArmRepository.GetArmByIdAsync(armId);
            if (robotArm == null) return null;

            return _mapper.Map<RobotArmDTO>(robotArm);
        }


        public async Task<List<RobotArmDTO>> GetAllArmsAsync()
        {
            // var arms = await _context.RobotArms.ToListAsync();
            var arms = await _robotArmRepository.GetAllArmsAsync();
            return _mapper.Map<List<RobotArmDTO>>(arms);
        }

        public async Task ResetArmAsync(int armId)
        {
            // var robotArm = await _context.RobotArms.FindAsync(armId);
            var robotArm = await _robotArmRepository.GetArmByIdAsync(armId);
            if (robotArm != null)
            {
                robotArm.PositionX = 0;
                robotArm.PositionY = 0;
                robotArm.PositionZ = 0;
                robotArm.Rotation = 0;
                
                robotArm.SetJointAngles(new List<float> { 0, 0, 0, 0, 0, 0 });

                // await _context.SaveChangesAsync();
                await _robotArmRepository.UpdateRobotArmAsync(robotArm);
                _logger.LogInformation($"Robot arm {armId} reset to default state.");
            }
        }

        public async Task<bool> DeleteArmAsync(int armId)
        {
            // var robotArm = await _context.RobotArms.FindAsync(armId);
            var robotArm = await _robotArmRepository.GetArmByIdAsync(armId);
            if (robotArm == null)
            {
                return false;
            }

            //_context.RobotArms.Remove(robotArm); await _context.SaveChangesAsync();
            await _robotArmRepository.DeleteArmAsync(armId);

            return true;
        }
    }
}
