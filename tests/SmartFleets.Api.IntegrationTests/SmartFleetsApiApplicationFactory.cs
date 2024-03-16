using Microsoft.AspNetCore.Mvc.Testing;
using SmartFleets.Api;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Ingestion.Api.IntegrationTests
{
    public class SmartFleetsApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .WithImage("rabbitmq:3-management")
            .Build();

        private readonly MsSqlContainer _msSql = new MsSqlBuilder()
            .Build();

        public async Task InitializeAsync()
        {

            await _msSql.StartAsync();
            Environment.SetEnvironmentVariable("ConnectionStrings:smartfleets_db", _msSql.GetConnectionString());

            await _rabbitMqContainer.StartAsync();
            Environment.SetEnvironmentVariable("ConnectionStrings:rabbitmq", _rabbitMqContainer.GetConnectionString());
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await base.DisposeAsync();
            await _msSql.DisposeAsync();
            await _rabbitMqContainer.DisposeAsync();
        }
    }
}
