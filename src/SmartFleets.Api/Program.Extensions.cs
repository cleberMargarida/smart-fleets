using ServiceModels;
using ServiceModels.Helpers;
using SmartFleets.Api;
using SmartFleets.Infrastructure.Consumers;
using SmartFleets.RabbitMQ.Messaging.Extensions;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace SmartFleets.Api
{
    [ExcludeFromCodeCoverage]
    public static class ProgramExtensions
    {
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
                cfg.Consume<VehicleHistoricalState>();

                foreach (var signalType in SignalTypeBindingHelper.TypeMapping.Values)
                {
                    cfg.Consume(signalType);
                }

            }, typeof(VehicleStateConsumer));

            return services;
        }
    }
}