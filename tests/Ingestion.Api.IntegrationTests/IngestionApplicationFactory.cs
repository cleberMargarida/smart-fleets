using Ingestion.Api.Handlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Ingestion.Api.IntegrationTests
{
    public class IngestionApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .WithImage("rabbitmq:3-management")
            .Build();

        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis/redis-stack-server:latest")
            .Build();

        private readonly WebApplicationFactory<Silo.Program> _orleansSilo = new WebApplicationFactory<Silo.Program>()
            .WithWebHostBuilder(builder => builder.Configure(app => { }));

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services => services.Configure<PublishHandlerConfiguration>(c => 
            {
                c.BatchSize = 0;
                c.Timeout = TimeSpan.Zero;
                c.ScanPeriod = TimeSpan.Zero;
            }));
        }

        public async Task InitializeAsync()
        {
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER", "true");

            await _rabbitMqContainer.StartAsync();
            Environment.SetEnvironmentVariable("ConnectionStrings:rabbitmq", _rabbitMqContainer.GetConnectionString());

            await _redisContainer.StartAsync();
            Environment.SetEnvironmentVariable("ConnectionStrings:redis", _redisContainer.GetConnectionString());

            _ = _orleansSilo.Services;
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await base.DisposeAsync();
            await _orleansSilo.DisposeAsync();
            await _rabbitMqContainer.DisposeAsync();
            await _redisContainer.DisposeAsync();
        }
    }
}



