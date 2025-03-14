using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using 


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

    [HttpGet("api/move")]
    public async Task<IActionResult> MoveArm([FromBody] MovementCommand command)
    {
        return "";
    }

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
    
    [HttpGet("stats")]
    public IActionResult GetArmStatus()
    {
        var status = 
        return Ok(status)
    }
}