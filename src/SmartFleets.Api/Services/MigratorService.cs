
using Microsoft.EntityFrameworkCore;
using SmartFleets.Infrastructure.Data;

namespace SmartFleets.Api.Services
{
    internal class MigratorService : StartOnlyService
    {
        private readonly IServiceScopeFactory _factory;

        public MigratorService(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _factory.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            var migrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
            if (migrations.Any())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
        }
    }
}