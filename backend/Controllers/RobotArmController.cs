using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RoboticArmSim.Models;
using RoboticArmSim.Services;
using RoboticArmSim.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace RoboticArmSim.Controllers;

// [Authorize]
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

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateArm([FromBody] CreateRobotArmDTO createDto)
    {
        var response = await _robotArmService.CreateRobotArmAsync(createDto);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to create robot arm: {errors}", string.Join(", ", response.ErrorMessages));
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetArmById), new { armId = response.Data.Id }, response);

    }

    [Authorize]
    [HttpPost("move")]
    public async Task<IActionResult> MoveArm([FromBody] MovementCommand command)
    {
        var result = await _robotArmService.MoveArmAsync(command);
        if (result == null)
            return BadRequest("Invalid movement command.");

        return Ok(result);
    }


    [HttpGet("state/{armId}")]
    public async Task<IActionResult> GetArmById(int armId)
    {
        var state = await _robotArmService.GetArmByIdAsync(armId);
        if (state == null) return NotFound("Robot Arm not found.");
        return Ok(state);

    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllArms()
    {
        var arms = await _robotArmService.GetAllArmsAsync();
        if (arms == null || arms.Count == 0) return NotFound("Robot Arm not found.");
        return Ok(arms);
    }

    [HttpPut("reset/{armId}")]
    public async Task<IActionResult> ResetArm(int armId)
    {
        await _robotArmService.ResetArmAsync(armId);
        return Ok($"Robot arm {armId} has been reset.");
    }

    [HttpDelete("delete/{armId}")]
    public async Task<IActionResult> DeleteArm(int armId)
    {
        var success = await _robotArmService.DeleteArmAsync(armId);
        if (!success)
        {
            return NotFound($"Robot arm with ID {armId} no found.");
        }
        return Ok($"Robot arm with ID {armId} deleted successfully");
    }
}


    
    
    