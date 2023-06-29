using Microsoft.AspNetCore.SignalR;
using Store.Helpers.Worker;
using Store.Hubs;

namespace Store
{
    public class Worker : BackgroundService
    {
        private readonly IWorkerService _workerService;
        private readonly IHubContext<ServerHub> _hubContext;

        public Worker(IWorkerService workerService, IHubContext<ServerHub> hubContext)
        {
            _workerService = workerService;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime hoy = DateTime.Now;
                var rolList = await _workerService.GetRolesAsync();
                foreach (var rol in rolList)
                {
                    if (
                        hoy.TimeOfDay.TotalDays >= rol.StartOperations.TimeOfDay.TotalDays
                        && hoy.TimeOfDay.TotalDays <= rol.EndOperations.TimeOfDay.TotalDays
                    )
                    {
                        if (rol.IsServerAccess == false)
                        {
                            rol.IsServerAccess = true;
                            await _workerService.UpdateRolSessionAsync(rol);
                            await _hubContext.Clients.All.SendAsync("serverAccess");
                        }
                    }
                    else
                    {
                        if (rol.IsServerAccess)
                        {
                            rol.IsServerAccess = false;
                            await _workerService.UpdateRolSessionAsync(rol);
                            await _hubContext.Clients.All.SendAsync("serverAccess");
                        }
                    }
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
