using Microsoft.AspNetCore.SignalR;

namespace Store.Hubs
{
    public class NewSalehub : Hub
    {
        public async Task SendSaleUpdate()
        {
            await Clients.All.SendAsync("saleUpdate");
        }
    }
}
