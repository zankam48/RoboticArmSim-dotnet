using Microsoft.AspNetCore.SignalR;

public class RoboticArmHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendArmUpdate(string update)
    {
        await Clients.All.SendAsync("ReceiveArmUpdate", update);
    }
}