using System.ComponentModel.DataAnnotations;

namespace RoboticArmSim.Models;

public class User
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Role {get; set;}
    public DateTime ConnectedAt {get; set;}
    public DateTime LastCommandTime {get; set;}
    public bool IsControlling {get; set;}
}