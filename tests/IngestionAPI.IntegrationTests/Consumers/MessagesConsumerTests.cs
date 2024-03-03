using IngestionAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using ServiceModels;
using SmartFleets.RabbitMQ.Base;

namespace IngestionAPI.IntegrationTests.Consumers;

public class MessagesConsumerTests :
    IClassFixture<IngestionApiApplicationFactory>,
    IClassFixture<StubConsumer>
{
    private readonly IngestionApiApplicationFactory _api;
    private readonly StubConsumer _consumer;

    public MessagesConsumerTests(IngestionApiApplicationFactory apiFactory, StubConsumer consumer)
    {
        _api = apiFactory;
        _consumer = consumer;
    }

    [Fact]
    public async Task ConsumeMessage_WithSpeedSignal_ShouldProcessAndPublishSignal()
    {
        // Arrange
        List<VehicleState> states = [];
        List<Speed> signals = [];
        var bus = _api.Services.GetRequiredService<IBus>();
        var message = new Message
        {
            Id = Guid.NewGuid().ToString(),
            Signals =
            {
                new()
                {
                    DateTimeUtc = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString(),
                    TenantId = 2,
                    Value = 1,
                    VehicleId = Guid.NewGuid().ToString(),
                    Type = 1,
                }
            }
        };
        _consumer.Connect(Environment.GetEnvironmentVariable("ConnectionStrings:rabbitmq"));
        _consumer.ConfigureTopology(t =>
        {
            t.Exchanges.WithFullName();
            t.Queues.WithPattern("IntegrationTest").WithFullName();
        });
        _consumer.Consume<Speed>(signals.Add);
        _consumer.Consume<VehicleState>(states.Add);

        // Act
        await bus.PublishAsync(message);
        await WrapContext();

        // Assert
        Assert.NotNull(signals[0]);
        Assert.Equal(message.Signals[0].DateTimeUtc, signals[0].DateTimeUtc);
        Assert.Equal(message.Signals[0].Id, signals[0].Id);
        Assert.Equal(message.Signals[0].TenantId, signals[0].TenantId);
        Assert.Equal(message.Signals[0].Value, signals[0].Value);
        Assert.Equal(message.Signals[0].VehicleId, signals[0].VehicleId);
        Assert.Equal(message.Signals[0].Type, (uint)signals[0].Type);
        Assert.NotNull(states[0]);
        Assert.Equal(message.Signals[0].DateTimeUtc, states[0].Speed.DateTimeUtc);
        Assert.Equal(message.Signals[0].Id, states[0].Speed.Id);
        Assert.Equal(message.Signals[0].TenantId, states[0].Speed.TenantId);
        Assert.Equal(message.Signals[0].Value, states[0].Speed.Value);
        Assert.Equal(message.Signals[0].VehicleId, states[0].Speed.VehicleId);
        Assert.Equal(message.Signals[0].Type, (uint)states[0].Speed.Type);
    }

    /// <summary>
    /// Wrap the context to consume the message.
    /// </summary>
    private static async Task WrapContext()
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
        get
        {
            return System.Diagnostics.Debugger.IsAttached;
        }
    }
}
