namespace RoboticArmSim.DTOs;

public class UserDTO
{
    public string Name { get; set; }
    public bool IsControlling { get; set; }
    public DateTime ConnectedAt { get; set; }
}