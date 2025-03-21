namespace RoboticArmSim.DTOs;

public class RobotArmDTO
{
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }
    public float Rotation { get; set; }
    public List<float> JointAngles { get; set; } = new List<float> {0f, 0f, 0f, 0f, 0f, 0f};
}