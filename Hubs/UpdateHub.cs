using Microsoft.AspNetCore.SignalR;

namespace Store.Hubs
{
    public class UpdateHub : Hub
    {
        public async Task ClientUpdate()
        {
            await Clients.All.SendAsync("updateClient");
        }
    }
}
