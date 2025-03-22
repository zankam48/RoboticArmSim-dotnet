namespace RoboticArmSim.Controllers;

using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RoboticArmSim.Models;
using System.Threading.Tasks;
using RoboticArmSim.Services;
using RoboticArmSim.DTOs;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    
    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(UserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    } 

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
    {
        var response = await ApiResponse<UserDTO>();

        if (!await _userService.IsUniqueUserAsync(model.Username))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages.Add("Username already exists");
            return BadRequest(response);
        }

        var user = await _userService.RegisterUserAsync(model);
        if (user == null)
        {
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.IsSuccess = false;
            response.ErrorMessages.Add("Failed to register user");
            return StatusCode(500, response);
        }

        response.StatusCode = HttpStatusCode.OK;
        response.IsSuccess = true;
        response.Data = user;
        return Ok(response);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var token = await _userService.AuthenticateAsync(request);
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Invalid credentials.");
        }
        return Ok(new {Token = token});
    }

    [HttpPost("assign-control")]
    public async Task<IActionResult> AssignControl([FromBody] User user)
    {
        var result = await _userService.AssignControlAsync(user.Id);
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


