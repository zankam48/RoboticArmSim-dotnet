using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Data;
using RoboticArmSim.Models;

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
    
    public async Task AddLogAsync(MovementLog log)
    {
        _context.MovementLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MovementLog>> GetLogsByUserAsync(int userId)
    {
        return await _context.MovementLogs
           .Where(l => l.UserId == userId)
           .OrderByDescending(l => l.TimeStamp)
           .ToListAsync();
    }

    public async Task<List<MovementLog>> GetRecentLogsAsync(int count = 10)
        {
            return await _context.MovementLogs
                .OrderByDescending(log => log.TimeStamp)
                .Take(count)
                .ToListAsync();
        }

    public async Task<PagedResult<MovementLog>> GetLogsPagedAsync(int page, int pageSize)
    {
        var query = _context.MovementLogs.OrderByDescending(log => log.TimeStamp);
        var totalCount = await query.CountAsync();

        var logs = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<MovementLog>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = logs
        };
    }
}

    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }



