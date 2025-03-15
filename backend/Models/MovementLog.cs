using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoboticArmSim.Models;
public class MovementLog
{
    [Key]
    public int Id {get; set;}

    [Required]
    public int RobotArmId {get; set;}

    [Required]
    public string Joint {get; set;}

    [Required]
    public float Angle {get; set;}    
    [Required]
    public string CommandType {get; set;}
    [Required]
    public DateTime TimeStamp {get; set;} = DateTime.Now;
    public int? UserId {get; set;}
    
    [ForeignKey("UserId")]
    public User? User {get; set;}

    [ForeignKey("RobotArmId")]
    public RobotArm RobotArm {get; set;}
}



