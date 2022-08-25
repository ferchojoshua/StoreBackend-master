using Microsoft.AspNetCore.SignalR;

namespace Store.Hubs
{
    public class NewFacturaHub : Hub
    {
        public async Task SendFactListUpdate()
        {
            await Clients.All.SendAsync("factListUpdate");
        }
    }
}
