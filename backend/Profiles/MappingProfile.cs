using AutoMapper;
using RoboticArmSim.Models;
using RoboticArmSim.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RobotArm, RobotArmDTO>();
        CreateMap<User, UserDTO>();
        CreateMap<MovementLog, MovementLogDTO>();
    }
}
