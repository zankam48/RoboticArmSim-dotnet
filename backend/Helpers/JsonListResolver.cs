using AutoMapper;
using System.Text.Json;
using RoboticArmSim.Models;
using RoboticArmSim.DTOs;

public class JsonToListResolver : IValueResolver<RobotArm, RobotArmDTO, List<float>>
{
    public List<float> Resolve(RobotArm source, RobotArmDTO destination, List<float> destMember, ResolutionContext context)
    {
        return JsonSerializer.Deserialize<List<float>>(source.JointAngles) ?? new List<float> { 0, 0, 0, 0, 0, 0 };
    }
}

public class ListToJsonResolver : IValueResolver<RobotArmDTO, RobotArm, string>
{
    public string Resolve(RobotArmDTO source, RobotArm destination, string destMember, ResolutionContext context)
    {
        return JsonSerializer.Serialize(source.JointAngles);
    }
}
