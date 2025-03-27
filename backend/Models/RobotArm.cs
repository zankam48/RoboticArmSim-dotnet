using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RoboticArmSim.Models;

public class RobotArm
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // public int Id { get; set; }
    public Guid Id { get; set; }

    [Required]
    public float PositionX { get; set; }

    [Required]
    public float PositionY { get; set; }

    [Required]
    public float PositionZ { get; set; }

    [Required]
    public float Rotation { get; set; }

    [Required]
    public string JointAngles { get; set; } = JsonSerializer.Serialize(new List<float> { 0f, 0f, 0f, 0f, 0f, 0f });

    public void SetJointAngles(List<float> angles)
    {
        JointAngles = JsonSerializer.Serialize(angles);
    }

    public List<float> GetJointAngles()
    {
        return JsonSerializer.Deserialize<List<float>>(JointAngles) ?? new List<float>();
    }
}
