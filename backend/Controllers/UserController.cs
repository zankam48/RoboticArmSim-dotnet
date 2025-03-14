namespace RoboticArmSim.Controllers;

using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RoboticArmSim.Models;
using System.Threading.Tasks;

public class UserController : Controller
{
    [ApiController]
    [Route("api/[controller]")]
    private readonly UserService _userService;

    public UserController(ILogger<UserController> userService)
    {
        _userService = userService;
    } 

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var result = await _userService.RegisterUserAsync(user);
        if (!result)
        {
            return BadRequest("User registration failed.");
        }
        return Ok("User registered succesfully.")
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _userService.AuthenticateAsync(request);
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Invalid credentials.");
        }
        return Ok(new {Token = token});
    }

    [HttpPost("assign-control")]
    public async Task<IActionResult> AssignControl([FromBody] ControlRequest request)
    {
        var result = await _userService.AssignControlAsync(request.UserId);
        if (!result)
        {
            return BadRequest("Control assignment failed.");
        }
        return Ok("Control assigned to user.");
    }

    [HttpGet("active-users")]
    public IActionResult GetActiveUsers()
    {
        var users = _userService.GetActiveUsers();
        return Ok(users);
    }

}


