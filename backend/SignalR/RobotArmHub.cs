using Microsoft.AspNetCore.SignalR;

public class RoboticArmHub : Hub
{
    private readonly ILogger<RoboticArmHub> _logger;
    public RoboticArmHub(ILogger<RoboticArmHub> logger)
    {
        _logger = logger;
    }
    public async Task SendMessage(string user, string message)
    {
        _logger.LogInformation($"Message from {user}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendArmUpdate(string update)
    {
        _logger.LogInformation($"Broadcasting robotic arm update: {update}");
        await Clients.All.SendAsync("ReceiveArmUpdate", update);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation($"Client connected: {Context.ConnectionId}");
        await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}