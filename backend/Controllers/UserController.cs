using Microsoft.AspNetCore.Mvc;
namespace RoboticArmSim.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    } 

    [HttpGet]


}