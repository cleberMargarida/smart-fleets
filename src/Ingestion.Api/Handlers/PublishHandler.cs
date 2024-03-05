using AutoMapper;
using Ingestion.Api.Handlers.Abstractions;
using Microsoft.Extensions.Options;
using ServiceModels.Abstractions;
using ServiceModels.Binding;
using SmartFleets.RabbitMQ.Base;
using System.Diagnostics;

namespace Ingestion.Api.Handlers
{
    /// <summary>
    /// An asynchronous handler that publishes signals to a message bus.
    /// </summary>
    public class PublishHandler : IAsyncHandler, IDisposable
    {
        private static readonly SignalDateTimeComparer _comparer = new();
        private readonly IBus _bus;
        private readonly IOptions<PublishHandlerConfiguration> _configuration;
        private Task? _periodicScan;
        private PeriodicTimer? _periodicTimer;

        private readonly Dictionary<SignalType, SortedSet<BaseSignal>> _sets
            = Enum.GetValues<SignalType>().ToDictionary(t => t, t => new SortedSet<BaseSignal>(_comparer));

        private readonly Dictionary<SignalType, Stopwatch> _clocks
            = Enum.GetValues<SignalType>().ToDictionary(t => t, t => new Stopwatch());

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishHandler"/> class.
        /// </summary>
        /// <param name="bus">The message bus for publishing signals.</param>
        /// <param name="configuration">The configuration options for the handler.</param>
        public PublishHandler(
            IBus bus,
            IOptions<PublishHandlerConfiguration> configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        /// <summary>
        /// Asynchronously handles a signal by adding it to the appropriate set and publishing it if necessary.
        /// </summary>
        /// <param name="current">The signal to be handled.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task HandleAsync(BaseSignal signal)
        {
            _periodicScan ??= Task.Run(PeriodicScan);
            var set = GetSet(signal);

            lock (set)
            {
                return HandleAsyncInternal(signal, signal.Type, set);
            }
        }

        private Task HandleAsyncInternal(SignalType type, SortedSet<BaseSignal> set, bool force = false)
        {
            lock (set)
            {
                return HandleAsyncInternal(null, type, set, force);
            }
        }

        private async Task HandleAsyncInternal(BaseSignal? signal, SignalType type, SortedSet<BaseSignal> set, bool force = false)
        {
            var stopwatch = _clocks[type];

            if (set.Count == 0)
            {
                stopwatch.Start();
            }

            if (signal is not null)
            {
                set.Add(signal);
            }

            if (ShouldSkip(type) && !force)
            {
                return;
            }

            await _bus.PublishSignalsAsync(set);

            set.Clear();
            stopwatch.Reset();
        }

        private SortedSet<BaseSignal> GetSet(BaseSignal signal)
        {
            return _sets[signal.Type];
        }

        private bool ShouldSkip(SignalType type)
        {
            bool batch = _configuration.Value.UseBatchSize &&
                _configuration.Value.BatchSize > _sets[type].Count;

            bool timeout = _configuration.Value.UseTimeout &&
                _configuration.Value.Timeout > _clocks[type].Elapsed;

            return batch || timeout;
        }

        private async Task PeriodicScan()
        {
            if (!_configuration.Value.UseScanPeriod)
            {
                return;
            }

            _periodicTimer = new PeriodicTimer(_configuration.Value.ScanPeriod);

            while (await _periodicTimer.WaitForNextTickAsync())
            {
                foreach (var (type, set) in _sets)
                {
                    if (set.Count != 0)
                    {
                        await HandleAsyncInternal(type, set, force: true);
                    }
                }
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="PublishHandler"/>.
        /// </summary>
        public void Dispose()
        {
            _periodicTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

