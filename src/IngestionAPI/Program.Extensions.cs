using IngestionAPI.Handlers.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using ServiceModels.Helpers;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace IngestionAPI
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
    }
}

#nullable disable
namespace Microsoft.Extensions.Hosting
{
    [ExcludeFromCodeCoverage]
    public static class HostExtensions
    {
        public static void UseOrleansClient(this ConfigureHostBuilder host)
        {
            host.UseOrleansClient((context, silo) =>
            {
                silo.UseConnectionRetryForever();

                if (context.HostingEnvironment.IsRunningInDocker())
                {
                    silo.UseRedisClustering(context.Configuration.GetConnectionString("redis"));
                }
                else
                {
                    silo.UseLocalhostClustering();
                }
            });
        }
    }

    public static class ClientBuilderExtensions
    {
        public static IClientBuilder UseConnectionRetryForever(this IClientBuilder silo)
        {
            return silo.UseConnectionRetryFilter((_, _) => Task.FromResult(true));
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
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

        public static IServiceCollection AddPipeline(this IServiceCollection services, Action<PipelineConfigurator> pipelineConfigurator)
        {
            var obj = new PipelineConfigurator();
            pipelineConfigurator(obj);
            foreach (var type in (HashSet<Type>)obj)
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHandler), type));
            }
            return services.AddSingleton<IPipeline, Pipeline>();
        }
    }

    static class SignalTypeBindingExtensions
    {
        public static Type DestinationType(this Signal signal)
        {
            return SignalTypeBindingHelper.TypeMapping[signal.Type];
        }
    }
}
