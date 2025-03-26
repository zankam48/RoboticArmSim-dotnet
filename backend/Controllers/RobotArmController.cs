using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RoboticArmSim.Models;
using RoboticArmSim.Services;
using RoboticArmSim.DTOs;


namespace RoboticArmSim.Controllers;

[ApiController] 
[Route("api/[controller]")]
public class RoboticArmController : ControllerBase
{
    private readonly RobotArmService _robotArmService;
    private readonly IHubContext<RoboticArmHub> _robotArmHub;
    private readonly ILogger<RoboticArmController> _logger;

    public RoboticArmController(ILogger<RoboticArmController> logger, RobotArmService robotArmService, IHubContext<RoboticArmHub> robotArmHub)
    {
        _logger = logger;
        _robotArmService = robotArmService;
        _robotArmHub = robotArmHub;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateArm([FromBody] RobotArmDTO robotArmDTO)
    {
        var createdArm = await _robotArmService.CreateRobotArmAsync(robotArmDTO);
        return Ok(createdArm);
    }

    [HttpPost("move")]
    public async Task<IActionResult> MoveArm([FromBody] MovementCommand command)
    {
        var result = await _robotArmService.MoveArmAsync(command);
        if (result == null)
            return BadRequest("Invalid movement command.");

        return Ok(result);
    }


    [HttpGet("state/{armId}")]
    public async Task<IActionResult> GetArmState(int armId)
    {
        var state = await _robotArmService.GetArmStateAsync(armId);
        if (state == null) return NotFound("Robot Arm not found.");
        return Ok(state);

    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllArms()
    {
        var arms = await _robotArmService.GetAllArmsAsync();
        if (arms == null) return NotFound("Robot Arm not found.");
        return Ok();
    }

    [HttpDelete("reset/{armId}")]
    public async Task<IActionResult> ResetArm(int armId)
    {
        await _robotArmService.ResetArmAsync(armId);
        return Ok($"Robot arm {armId} has been reset.");
    }
}


    
    
    