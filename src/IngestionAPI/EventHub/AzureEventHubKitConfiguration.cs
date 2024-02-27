using Azure.Messaging.EventHubs;
using IngestionAPI.EventHub;
using IngestionAPI.EventHub.Strategies;

#nullable disable

namespace Microsoft.Extensions.DependencyInjection
{
    public class AzureEventHubKitConfiguration
    {
        internal AzureEventHubConfiguration AzureEventHubConfiguration { get; set; } = default!;
        internal CheckpointStrategyFactory CheckpointStrategy { get; set; } = default!;

        public void Checkpoint(Action<CheckpointStrategyFactory> configure)
        {
            configure(CheckpointStrategy ??= new CheckpointStrategyFactory());
        }

        public void ConfigureEventHub(Action<AzureEventHubConfiguration> configure)
        {
            configure(AzureEventHubConfiguration ??= new AzureEventHubConfiguration());
        }

        internal EventProcessorClient BuildEventProcessorClient()
        {
            if (AzureEventHubConfiguration.ConnectionString is not null)
            {
                return new EventProcessorClientImpl(
                       AzureEventHubConfiguration.BlobContainerClient,
                       AzureEventHubConfiguration.ConsumerGroupName,
                       AzureEventHubConfiguration.ConnectionString,
                       AzureEventHubConfiguration.EventProcessorClientOptions,
                       CheckpointStrategy);
            }

            return new EventProcessorClientImpl(
                   AzureEventHubConfiguration.BlobContainerClient,
                   AzureEventHubConfiguration.ConsumerGroupName,
                   AzureEventHubConfiguration.Namespace,
                   AzureEventHubConfiguration.Name,
                   AzureEventHubConfiguration.Credential,
                   AzureEventHubConfiguration.EventProcessorClientOptions,
                   CheckpointStrategy);

        }
    }

}
