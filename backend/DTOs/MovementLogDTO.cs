namespace RoboticArmSim.DTOs;

public class MovementLogDTO
{
    public Guid Id { get; set; }
    public string Joint { get; set; }
    public float Angle { get; set; }
    public string CommandType { get; set; }
    public DateTime TimeStamp { get; set; }
}
