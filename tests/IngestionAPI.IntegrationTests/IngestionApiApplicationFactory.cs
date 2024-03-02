using IngestionAPI.Handlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace IngestionAPI.IntegrationTests
{
    public class IngestionApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder().WithImage("rabbitmq:3-management").Build();
        private readonly RedisContainer _redisContainer = new RedisBuilder().WithImage("redis/redis-stack-server:latest").Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            Environment.SetEnvironmentVariable("ConnectionStrings:redis", _redisContainer.GetConnectionString());
            Environment.SetEnvironmentVariable("ConnectionStrings:rabbitmq", _rabbitMqContainer.GetConnectionString());
            builder.ConfigureServices(services => 
                services.Configure<PublishHandlerConfiguration>(c => c.Timeout = TimeSpan.FromMilliseconds(100)));
        }

        public async Task InitializeAsync()
        {
            await _rabbitMqContainer.StartAsync();
            await _redisContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await base.DisposeAsync();
            await _rabbitMqContainer.DisposeAsync();
            await _redisContainer.DisposeAsync();
        }
    }
}


