using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.RabbitMq;
using IContainer = DotNet.Testcontainers.Containers.IContainer;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace IngestionAPI.IntegrationTests
{
    public class IngestionApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithPortBinding(15672, true)
                .WithPortBinding(5672, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                .Build();

        private readonly IContainer _redisContainer = new ContainerBuilder()
                .WithImage("redis/redis-stack-server:latest")
                .WithPortBinding(6379, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
                .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            Environment.SetEnvironmentVariable("ConnectionStrings:redis", $"{_redisContainer.Hostname}:{_redisContainer.GetMappedPublicPort(6379)}");
            Environment.SetEnvironmentVariable("ConnectionStrings:rabbitmq", _rabbitMqContainer.GetConnectionString());
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


