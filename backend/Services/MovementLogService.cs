using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Data;
using RoboticArmSim.DTOs;
using RoboticArmSim.Models;

namespace RoboticArmSim.Services;

public class MovementLogService
{
    private readonly ILogger<MovementLogService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MovementLogService(ILogger<MovementLogService> logger, ApplicationDbContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
    
    public async Task AddLogAsync(MovementLog log)
    {
        try
        {
            await _context.MovementLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            
            _logger.LogError($"Failed to add movement log: {ex.Message}");
        }
    }

    public async Task<List<MovementLogDTO>> GetLogsByUserAsync(int userId)
    {
        var logs = await _context.MovementLogs
           .Where(l => l.UserId == userId)
           .OrderByDescending(l => l.TimeStamp)
           .ToListAsync();

        return _mapper.Map<List<MovementLogDTO>>(logs); 
    }

    public async Task<List<MovementLogDTO>> GetRecentLogsAsync(int count = 10)
        {
            var logs = await _context.MovementLogs
                .OrderByDescending(log => log.TimeStamp)
                .Take(count)
                .ToListAsync();
            
            return _mapper.Map<List<MovementLogDTO>>(logs);
        }

    public async Task<PagedResult<MovementLogDTO>> GetLogsPagedAsync(int page, int pageSize)
    {
        var query = _context.MovementLogs.OrderByDescending(log => log.TimeStamp);
        var totalCount = await query.CountAsync();

        var logs = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<MovementLogDTO>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = _mapper.Map<List<MovementLogDTO>>(logs)
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



