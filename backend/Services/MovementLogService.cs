using RoboticArmSim.Data;

namespace RoboticArmSim.Services;

public class MovementLogService
{
    private readonly ILogger<MovementLogService> _logger;
    private readonly ApplicationDbContext _context;

    public MovementLogService(ILogger<MovementLogService> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

}