using ServiceModels.Abstractions;
using SmartFleets.RabbitMQ.Base;
using System.Collections;
using System.Reflection;

#nullable disable

namespace Ingestion.Api.Handlers
{
    static class BusExtensions
    {
        private static readonly MethodInfo _publishMethod
            = typeof(IBus).GetMethod(nameof(IBus.PublishBatchAsync));

        private static readonly MethodInfo _collectionMethod
            = typeof(BusExtensions).GetMethod(nameof(Collection), BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        ///  Publishes a batch of signals of a specific type asynchronously.
        /// </summary>
        /// <param name="bus">The bus instance.</param>
        /// <param name="signals">The signals to publish.</param>
        /// <returns> A task that represents the asynchronous publish operation.</returns>
        public static Task PublishSignalsAsync(this IBus bus, SortedSet<BaseSignal> signals)
        {
            var type = signals.First().GetType();
            var values = _collectionMethod.MakeGenericMethod(type).Invoke(null, new[] { signals });
            return (Task)_publishMethod.MakeGenericMethod(type).Invoke(bus, new [] { values });
        }

        private static IEnumerable<T> Collection<T>(SortedSet<BaseSignal> signals) where T : BaseSignal
        {
            foreach (BaseSignal signal in signals)
            {
                yield return (T)signal;
            }
        }
    }
}