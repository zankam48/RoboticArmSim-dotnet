using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RoboticArmSim.Models;


namespace RoboticArmSim.Controllers;

[ApiController] 
[Route("api/[controller]")]
public class RoboticArmController : ControllerBase
{
    // robot arm service
    // ihubcontext 
    private readonly ILogger<RoboticArmController> _logger;

    public RoboticArmController(ILogger<RoboticArmController> logger)
    {
        _logger = logger;
    }

    [HttpGet("move")]
    public async Task<IActionResult> MoveArm([FromBody] MovementCommand command)
    {
        if (command.Angle < 0)
            return BadRequest("Angle must be positive!");

        var result = await _robotArmService.MoveArmAsync(command);
        if (result == null)
            return BadRequest("Invalid movement command.");

        // Broadcast update to all connected clients
        await _robotArmHub.Clients.All.SendAsync("ReceiveArmUpdate", result);

        return Ok(result);
    }


    [HttpGet("state")]
    public IActionResult State()
    {
        return Ok();
    }
    [HttpGet("stats")]
    public IActionResult GetArmStatus()
    {
        return Ok();
    }
}


    [HttpPatch("update")]
    public IActionResult Update()
    {
        return Ok();
    }


    [HttpDelete("reset")]
    public IActionResult Reset()
    {
        return Ok();
    }
    
    