using IngestionAPI.Models;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceModels;
using SmartFleets.RabbitMQ.Base;

namespace IngestionAPI.IntegrationTests.Consumers;

public class MessagesConsumerTests : IClassFixture<IngestionApiApplicationFactory>
{
    private static readonly string _topology = typeof(Speed).FullName!;
    private readonly IngestionApiApplicationFactory _apiFactory;

    public MessagesConsumerTests(IngestionApiApplicationFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }

    [Fact]
    public async Task ConsumeMessage_WithSpeedSignal_ShouldProcessAndPublishSignal()
    {
        // Arrange
        await _apiFactory.InitializeAsync();
        Signal expectedSignal = new()
        {
            DateTimeUtc = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString(),
            TenantId = 2,
            Value = 1,
            VehicleId = Guid.NewGuid().ToString(),
            Type = 1,
        };
        Message message = new()
        {
            Id = Guid.NewGuid().ToString(),
            Signals = [expectedSignal]
        };
        Speed? actualSignal = null;
        var bus = _apiFactory.Services.GetRequiredService<IBus>();
        var connection = _apiFactory.Services.GetRequiredService<IConnection>();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(_topology, autoDelete: false);
        channel.ExchangeDeclare(_topology, ExchangeType.Topic, durable: true, autoDelete: false);
        channel.QueueBind(_topology, _topology, "#");
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += (_, e) => { actualSignal = MessagePackSerializer.Deserialize<Speed>(e.Body); return Task.CompletedTask; };
        channel.BasicConsume(_topology, false, consumer);

        // Act
        await bus.PublishAsync(message);
        await Task.Delay(300);
        await bus.PublishAsync(message);
        await Task.Delay(300);

        // Assert
        Assert.NotNull(actualSignal);
        Assert.Equal(expectedSignal.DateTimeUtc, actualSignal.DateTimeUtc);
        Assert.Equal(expectedSignal.Id, actualSignal.Id);
        Assert.Equal(expectedSignal.TenantId, actualSignal.TenantId);
        Assert.Equal(expectedSignal.Value, actualSignal.Value);
        Assert.Equal(expectedSignal.VehicleId, actualSignal.VehicleId);
        Assert.Equal(expectedSignal.Type, (uint)actualSignal.Type);
    }
}
