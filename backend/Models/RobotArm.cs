using System.ComponentModel.DataAnnotations;

namespace RoboticArmSim.Models;

public class RobotArm
{
    public int Id {get; set;}
    public float PositionX {get; set;}
    public float PositionY {get; set;}
    public float PositionZ {get; set;}
    public float Rotation {get; set;}
    public List<float> JointAngles {get; set;}
}