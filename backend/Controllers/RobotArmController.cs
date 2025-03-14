using Microsoft.AspNetCore.Mvc;

namespace RoboticArmSim.Controllers;
public class RoboticArmController : Controller
{
    private readonly ILogger<RoboticArmController> _logger;

    public RoboticArmController(ILogger<RoboticArmController> logger)
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

    [HttpGet("api/robotarm/state")]
    public IActionResult State()
    {

    }
    

    [HttpPost("api/robotarm/move")]
    public IActionResult MoveArm(int armId, int position)
    {
        if (position < 0) return BadRequest("Position must be positive!");
        return Ok($"Arm {armId} moved to position {position}");
    }

    [HttpUpdate("api/robotarm/update")]
    public IActionResult Update()
    {

    }


    [HttpDelete("api/robotarm/reset")]
    public IActionResult Reset()
    {

    }
    
}