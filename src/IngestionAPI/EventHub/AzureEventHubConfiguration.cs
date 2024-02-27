using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;

#nullable disable

namespace Microsoft.Extensions.DependencyInjection
{
    public class AzureEventHubConfiguration
    {
        internal BlobContainerClient BlobContainerClient { get; set; } = default!;
        internal EventProcessorClientOptions EventProcessorClientOptions { get; set; } = default!;
        public string ConsumerGroupName { internal get; set; } = default!;
        public string ConnectionString { internal get; set; } = default!;
        public string Namespace { internal get; set; } = default!;
        public string Name { internal get; set; } = default!;
        public TokenCredential Credential { internal get; set; } = default!;

        public void SetBlobContainer(string uri, TokenCredential azureCredential)
        {
            BlobContainerClient = new BlobContainerClient(new Uri(uri), azureCredential);
        }

        public void SetBlobContainer(string connectionString, string blobContainerName)
        {
            BlobContainerClient = new BlobContainerClient(connectionString, blobContainerName);
        }

        public void ConfigureClientOptions(Action<EventProcessorClientOptions> configure)
        {
            configure(EventProcessorClientOptions ??= new EventProcessorClientOptions());
        }
    }

}
