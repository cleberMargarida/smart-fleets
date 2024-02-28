using Polly;
using ServiceModels.Helpers;
using StackExchange.Redis;

namespace IngestionAPI
{
    public partial class Program
    {
    }
}

#nullable disable

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ProgramExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddRabbitMq((s, cfg) =>
            {
                cfg.Host(s.GetService<IConfiguration>().GetConnectionString("rabbitmq"));
                cfg.Topology(t =>
                {
                    t.Queues.WithFullName();
                    t.RoutingKeys.WithPattern("#");
                    t.Exchanges.WithFullName();
                });

                cfg.Consume<Message>();

                foreach (var signalType in SignalTypeBindingHelper.TypeMapping.Values)
                {
                    cfg.Produce(signalType);
                }
            });

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, Action<ConfigurationOptions> configure = null)
        {
            return services.AddSingleton(s =>
            {
                ConnectionMultiplexer muxer = null;
                Policy.Handle<Exception>().WaitAndRetryForever(_ => TimeSpan.FromSeconds(3)).Execute(() =>
                {
                    var configuration = s.GetService<IConfiguration>().GetConnectionString("redis");
                    if (configure is null)
                    {
                        muxer = ConnectionMultiplexer.Connect(configuration);
                    }
                    else
                    {
                        muxer = ConnectionMultiplexer.Connect(configuration, configure);
                    }
                });

                ArgumentNullException.ThrowIfNull(muxer);
                return muxer;
            }).AddSingleton<IConnectionMultiplexer>(s => s.GetRequiredService<ConnectionMultiplexer>());
        }
    }

    static class SignalTypeBindingExtensions
    {
        public static Type DestinationType(this Signal signal)
        {
            return SignalTypeBindingHelper.TypeMapping[signal.SignalType];
        }
    }
}
