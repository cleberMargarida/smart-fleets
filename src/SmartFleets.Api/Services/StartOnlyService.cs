namespace SmartFleets.Api.Services
{
    internal abstract class StartOnlyService : IHostedService
    {
        public abstract Task StartAsync(CancellationToken cancellationToken);
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}