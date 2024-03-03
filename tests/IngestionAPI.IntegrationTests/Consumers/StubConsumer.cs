using MessagePack;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartFleets.RabbitMQ.Messaging.Topology;

namespace IngestionAPI.IntegrationTests.Consumers
{
    public class StubConsumer : IDisposable
    {
        private readonly TopologyBuilderAggregator _topology = new();
        private readonly HashSet<Type> _types = [];

        private IConnection _connection;
        private IModel _channel;

        private bool _initialized;

        public void Connect(string connectionString)
        {
            Uri uri = new(connectionString);
            var factory = new ConnectionFactory { Uri = uri };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _initialized = true;
        }
  
        public void ConfigureTopology(Action<TopologyBuilderAggregator> configureTopology)
        {
            configureTopology(_topology);
        }

        public void Consume<T>(Action<T> consumingAction)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException($"The Stub was not initialized. Call {nameof(StubConsumer)}.{nameof(Connect)} first.");
            }

            if (!_types.Add(typeof(T)))
            {
                throw new Exception($"{typeof(T)} Already added. StubConsumer cannot handle multiples registration for the same type.");
            }

            var queue = _topology.Queues.Build([typeof(T)])[0];
            var exchange = _topology.Exchanges.Build([typeof(T)])[0];

            _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true, autoDelete: false);
            _channel.QueueBind(queue, exchange, "#", default);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (_, e) =>
            {
                var body = MessagePackSerializer.Deserialize<T>(e.Body);
                consumingAction(body);
            };

            _channel.BasicConsume(queue, false, consumer);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}