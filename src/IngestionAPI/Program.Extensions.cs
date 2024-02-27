using Azure.Messaging.EventHubs.Consumer;
using IngestionAPI.Models;
using Polly;
using ServiceModels.Abstractions;
using ServiceModels.Binding;
using StackExchange.Redis;
using System.Reflection;

namespace IngestionAPI
{
    public class Program
    {
    }
}

#nullable disable

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ProgramExtensions
    {
        public static IServiceCollection AddIngestionEventHub(this IServiceCollection services)
        {
            return services.AddAzureEventHub((s, cfg) =>
            {
                cfg.ConfigureEventHub(eventHub =>
                {
                    eventHub.ConnectionString = s.GetService<IConfiguration>().GetConnectionString("AzureEventHub");
                    eventHub.ConsumerGroupName = EventHubConsumerClient.DefaultConsumerGroupName;
                    eventHub.SetBlobContainer(s.GetService<IConfiguration>().GetConnectionString("BlobStorage"), s.GetService<IConfiguration>()["BlobContainerName"]!);
                });
                cfg.Checkpoint(c => c.ByCount(0));
            }).AddEventHandler();
        }

        public static IServiceCollection AddAzureEventHub(
            this IServiceCollection services, Action<IServiceProvider, AzureEventHubKitConfiguration> configure)
        {
            return services.AddSingleton(s =>
            {
                var configuration = new AzureEventHubKitConfiguration();
                configure(s, configuration);
                return configuration.BuildEventProcessorClient();
            });
        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
        {
            services.AddRabbitMq((s, cfg) =>
            {
                cfg.Host(s.GetService<IConfiguration>().GetConnectionString("rabbitmq"));
                cfg.Topology(t => t.Exchanges.WithFullName());

                foreach (var messageType in SignalTypeBinding.TypeMapping.Values)
                {
                    cfg.Produce(messageType);
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

    public static class SignalTypeBindingExtensions
    {
        public static Type DestinationType(this Signal signal)
        {
            return SignalTypeBinding.TypeMapping[signal.SignalType];
        }
    }

    public static class SignalTypeBinding
    {
        private static IReadOnlyDictionary<uint, Type> _typeMapping;
        public static IReadOnlyDictionary<uint, Type> TypeMapping => _typeMapping ??= typeof(SignalTypeBindingExtensions)
                .Assembly
                .DefinedTypes
                .Where(t => typeof(SignalAbstract).IsAssignableFrom(t) && !t.IsAbstract)
                .ToDictionary(t => t.GetCustomAttribute<TypeIdAttribute>()!.TypeAsNumber,
                              t => t.AsType())
                .AsReadOnly();
    }

}
