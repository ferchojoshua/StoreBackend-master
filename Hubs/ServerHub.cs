using Microsoft.AspNetCore.SignalR;

namespace Store.Hubs
{
    public class ServerHub : Hub
    {
        public async Task ServerAccess()
        {
            await Clients.All.SendAsync("serverAccess");
        }
    }
}
