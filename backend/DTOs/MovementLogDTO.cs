namespace RoboticArmSim.DTOs;

public class MovementLogDTO
{
    public int RobotArmId { get; set; }
    public string Joint { get; set; }
    public float Angle { get; set; }
    public string CommandType { get; set; }
    public DateTime TimeStamp { get; set; }
}
