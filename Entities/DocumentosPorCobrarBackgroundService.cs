using Store.Helpers.ReportHelper;

public class DocumentosPorCobrarBackgroundService : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DocumentosPorCobrarBackgroundService> _logger;
    private Timer _timer;

    public DocumentosPorCobrarBackgroundService(
        IServiceProvider services,
        ILogger<DocumentosPorCobrarBackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Servicio de Documentos por Cobrar iniciado: {time}", DateTimeOffset.Now);
        //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        try
        {
            _logger.LogInformation("Iniciando proceso de generación: {time}", DateTimeOffset.Now);

            using var scope = _services.CreateScope();
            var reportsHelper = scope.ServiceProvider.GetRequiredService<IReportsHelper>();
            var resultado = await reportsHelper.GenerarHistoricoDocumentosPorCobrarAsync();

            _logger.LogInformation(
                "Proceso completado: {time}. Resultado: {resultado}",
                DateTimeOffset.Now,
                resultado ? "Éxito" : "No se generaron nuevos registros"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en la generación de documentos por cobrar: {time}", DateTimeOffset.Now);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Servicio de Documentos por Cobrar detenido: {time}", DateTimeOffset.Now);
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();

        return Task.CompletedTask;
    }
}