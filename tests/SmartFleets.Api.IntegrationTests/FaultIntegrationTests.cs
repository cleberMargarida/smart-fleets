using Ingestion.Api.IntegrationTests;
using Microsoft.Extensions.DependencyInjection;
using ServiceModels;
using ServiceModels.Binding;
using SmartFleets.Application.Commands.CreateFaultMetadata;
using SmartFleets.Domain.Entities;
using SmartFleets.RabbitMQ.Base;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SmartFleets.Api.IntegrationTests;

public class FaultIntegrationTests :
    IClassFixture<SmartFleetsApiApplicationFactory>
{
    private readonly SmartFleetsApiApplicationFactory _api;

    public FaultIntegrationTests(SmartFleetsApiApplicationFactory apiFactory)
    {
        _api = apiFactory;
    }

    [Fact]
    public async Task PostFaultMetadataAndPublishVehicleState_WithSpeedExceedsThreshold_FaultDetected()
    {
        // Arrange
        var client = _api.CreateClient();
        var bus = _api.Services.GetService<IBus>();

        // Create a vehicle state object representing the current state of a vehicle.
        // This includes a speed reading indicating the vehicle is traveling at 100km/h, a speed capable to trigger the fault above.
        var state = new VehicleState
        {
            Speed = new Speed
            {
                DateTimeUtc = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                TenantId = 2,
                Value = 100,// Fault speed value.
                VehicleId = "PQP-2024"
            }
        };

        // Create a fault metadata object representing the criteria for detecting a
        // fault. This includes a description, a predicate that defines
        // the fault condition i.e (speed above 50km/h), and the associated signal type.
        var metadata = new CreateFaultMetadataRequest(
            Description: "Speed above 50km/h",
            Enabled: true,
            PredicateAsString: "state.Speed.Value > 50",
            SignalTypes: new[] { SignalType.Speed });

        // Serializing the object to JSON
        var body = JsonSerializer.Serialize(metadata);

        // Creating an HttpContent instance from the JSON string
        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");

        // Act
        // Insert the fault metadata into the database. This sets up the criteria
        // for detecting a speed-related fault in the system.
        var response = await client.PostAsync("/api/FaultMetadata", httpContent);

        // Publish the vehicle state to the message bus. This simulates the process
        // of receiving a new speed reading from a vehicle.
        await bus.PublishAsync(state);

        // Simulate the processing of the published vehicle state by sleeping this thread.
        // This is necessary because when a message is published through RabbitMQ, it is
        // consumed and processed by a separate thread or process managed by the .NET thread pool.
        // This parallel processing occurs independently of the thread that published the message.
        // By pausing the current thread with WrapContextAsync(), we create an opportunity to
        // debug the message consumption process in real-time, allowing us to observe the effects
        // of the consumption (such as fault detection) as they happen.
        await WrapContextAsync();

        // Retrieve the faults detected for the vehicle on the current day.
        // This checks if the fault detection logic correctly identified a fault
        // based on the speed reading exceeding the threshold defined in the metadata.
        var faults = await client.GetFromJsonAsync<Fault[]>($"/api/Faults/todays/{state.Speed.VehicleId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotEmpty(faults);
        Assert.False(faults[0].Id == default);
        Assert.Equal(faults[0].SignalId, state.Speed.Id);
        Assert.Equal(faults[0].CreatedAt, state.Speed.DateTimeUtc);
        Assert.Equal(faults[0].VehicleId, state.Speed.VehicleId);
        Assert.Equal(faults[0].SignalTypes, metadata.SignalTypes);
        Assert.Equal(faults[0].Description, metadata.Description);
    }

    /// <summary>
    /// Wrap the context to consume the message.
    /// </summary>
    private static async Task WrapContextAsync()
    {
        if (IsDebugging)
        {
            // Put here any time that you need.
            await Task.Delay(10000);
        }
        else
        {
            await Task.Delay(2000);
        }
    }

    private static bool IsDebugging
    {
        get => System.Diagnostics.Debugger.IsAttached;
    }
}

