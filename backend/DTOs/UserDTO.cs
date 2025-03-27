namespace RoboticArmSim.DTOs;

public class UserDTO
{
    // public string? Id {get; set;}
    public Guid Id {get; set;}
    public string? Username { get; set; }
    public bool IsControlling { get; set; }
    public DateTime ConnectedAt { get; set; }
}