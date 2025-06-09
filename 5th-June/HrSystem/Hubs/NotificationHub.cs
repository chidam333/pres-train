using Microsoft.AspNetCore.SignalR;

namespace HrSystem.Hubs;

public class NotificationHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
    public async Task SendFileUploadNotification(string message)
    {
        await Clients.All.SendAsync("FileUploaded", message);
    }
}