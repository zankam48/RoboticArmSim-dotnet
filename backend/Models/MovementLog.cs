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
    public DateTime TimeStamp {get; set;}
    public int? UserId {get; set;}
    
    [ForeignKey["UserId"]]
    public User? User;
}



namespace RoboticArmSimulation.API.Models
{
    public class MovementLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RobotArmId { get; set; } // Link to the Robotic Arm

        [Required]
        public string Joint { get; set; } // E.g., "Base", "Shoulder", "Elbow", etc.

        [Required]
        public float Angle { get; set; } // The angle moved in degrees

        [Required]
        public string CommandType { get; set; } // E.g., "Move", "Rotate", "Stop"

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Stores when the movement happened

        public int? UserId { get; set; } // Nullable - might not always have a user (e.g., automated movement)
        
        [ForeignKey("UserId")]
        public User? User { get; set; } // Navigation property

        [ForeignKey("RobotArmId")]
        public RobotArm RobotArm { get; set; } // Navigation property
    }
}
