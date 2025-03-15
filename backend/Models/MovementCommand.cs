namespace RoboticArmSim.Models;

public class MovementCommand
{
    public int ArmId { get; set; }
    public string Joint { get; set; }
    public float Angle { get; set; }
}