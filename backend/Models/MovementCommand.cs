namespace RoboticArmSim.Models;

public class MovementCommand
{
    public int ArmId { get; set; }
    public int Joint { get; set; }
    public float Angle { get; set; }
}