using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Store.Hubs;

namespace Store.Helpers.ServerHelper
{
    public class ServerService : IServerService
    {
        private readonly IConfiguration _configuration;
        private readonly IHubContext<ServerHub> _hubContext;

        public ServerService(IConfiguration configuration, IHubContext<ServerHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
        }

        public void Config()
        {
            string conString;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                conString = _configuration.GetConnectionString("DevConnetion");
            }
            else
            {
                conString = _configuration.GetConnectionString("ProdConnetion");
            }
            SqlDependency.Start(conString);
            ChangeWhatcher();
        }

        private void ChangeWhatcher()
        {
            string conString = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                conString = _configuration.GetConnectionString("DevConnetion");
            }
            else
            {
                conString = _configuration.GetConnectionString("ProdConnetion");
            }

            using (var conn = new SqlConnection(conString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"SELECT IsServerAccess FROM [DBO].Rols", conn))
                {
                    // cmd.Notification = null;
                    SqlDependency dependency = new SqlDependency(cmd);
                    // dependency.OnChange += DetectChange;
                    dependency.OnChange += new OnChangeEventHandler(DetectChange);
                    // SqlDependency.Start(conString);
                    var result = cmd.ExecuteReader();
                }
            }
        }

        private void DetectChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                _hubContext.Clients.All.SendAsync("serverAccess");
            }
            ChangeWhatcher();
        }
    }
}
