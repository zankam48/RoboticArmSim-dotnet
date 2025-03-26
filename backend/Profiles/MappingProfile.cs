using AutoMapper;
using RoboticArmSim.Models;
using RoboticArmSim.DTOs;
using System.Text.Json;

namespace RoboticArmSim;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RobotArm, RobotArmDTO>()
                .ForMember(dest => dest.JointAngles, opt => opt.MapFrom<JsonToListResolver>());

        CreateMap<RobotArmDTO, RobotArm>()
            .ForMember(dest => dest.JointAngles, opt => opt.MapFrom<ListToJsonResolver>());

        CreateMap<User, UserDTO>();
        CreateMap<MovementLog, MovementLogDTO>();
    }
}
