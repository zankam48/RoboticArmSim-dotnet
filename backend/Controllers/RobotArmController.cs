using Microsoft.AspNetCore.Mvc;

namespace RoboticArmSim.Controllers;
public class RoboticArmSimController : Controller
{
    private readonly ILogger<RoboticArmSimController> _logger;

    public RoboticArmSimController(ILogger<RoboticArmSimController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return "Welcome to Robotic Arm Simulation API!";
    }

    // [HttpGet("api/move")]
    // public ActionResult<string> MoveArm(int armId, int position)
    // {
    //     return "";
    // }

    [HttpGet("api/move")]
    public IActionResult MoveArm(int armId, int position)
    {
        if (position < 0) return BadRequest("Position must be positive!");
        return Ok($"Arm {armId} moved to position {position}");
    }
    
}