using Microsoft.AspNetCore.SignalR;

namespace Store.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("alertaCampana", "Este es un mensaje");
        }
    }
}
