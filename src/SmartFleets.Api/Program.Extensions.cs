using ServiceModels;
using ServiceModels.Helpers;
using SmartFleets.Api.Services;
using SmartFleets.Infrastructure.Consumers;
using SmartFleets.RabbitMQ.Messaging.Extensions;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace SmartFleets.Api
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public partial class Program { }

    /// <summary>
    /// Provides extension methods for configuring the application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ProgramExtensions
    {
        /// <summary>
        /// Adds the database context migrator as a hosted service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddDbContextMigrator(this IServiceCollection services)
        {
            return services.AddHostedService<MigratorService>();
        }

        /// <summary>
        /// Configures RabbitMQ with the application's messaging requirements.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddRabbitMq((s, cfg) =>
            {
                cfg.Host(s.GetService<IConfiguration>().GetConnectionString("rabbitmq"));
                cfg.Topology(t =>
                {
                    t.Queues.WithDomainName().WithFullName();
                    t.RoutingKeys.WithPattern("#");
                    t.Exchanges.WithFullName();
                });

                cfg.Consume<VehicleState>();

                foreach (var signalType in SignalTypeBindingHelper.TypeMapping.Values)
                {
                    cfg.Consume(signalType);
                }

            }, typeof(VehicleStateConsumer));

            return services;
        }
    }
}
