using Microsoft.AspNetCore.Mvc;


namespace RoboticArmSim.Controllers;

[ApiController] 
[Route("api/[controller]")]
public class RoboticArmController : ControllerBase
{
    // robot arm service
    // ihubcontext 
    // ilogger

    public RoboticArmController(ILogger<RoboticArmController> logger)
    {
        // yg 3-3nya masukin sini
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
        return Ok();
    }
    

    [HttpPost("move")]
    public async Task<IActionResult> MoveArm(int armId, int position)
    {
        if (position < 0) return BadRequest("Position must be positive!");
        return Ok($"Arm {armId} moved to position {position}");
    }

    [HttpPatch("api/robotarm/update")]
    public IActionResult Update()
    {
        return Ok();
    }


    [HttpDelete("api/robotarm/reset")]
    public IActionResult Reset()
    {
        return Ok();
    }
    
}