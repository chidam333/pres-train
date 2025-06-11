using Microsoft.AspNetCore.Mvc;
using ELearnApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/notify")]
public class NotifyController : ControllerBase
{
    private readonly IHubContext<NotifyHub> _hubContext;

    public NotifyController(IHubContext<NotifyHub> hubContext)

    {
        _hubContext = hubContext;
    }

    [HttpPost("send")]
    [Authorize]
    public async Task<IActionResult> SendNotification()
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", "This is a test notification");
        return Ok("Notification sent successfully.");
    }
}