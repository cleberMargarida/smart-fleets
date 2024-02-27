using IngestionAPI.EventHub;
using IngestionAPI.EventHub.MappingProfile;
using IngestionAPI.Handlers.MessageErrorHandling;
using IngestionAPI.Handlers.MessageEventHandling;
using IngestionAPI.Handlers.MessageEventValidation;
using IngestionAPI.Handlers.SignalEventHandling;
using IngestionAPI.Handlers.SignalEventValidation;
using IngestionAPI.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

#nullable disable

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventHubDependencyInjectionExtensions
    {
        private static readonly IReadOnlyCollection<Type> _messageValidators;
        private static readonly IReadOnlyCollection<Type> _messageHandlers;
        private static readonly IReadOnlyCollection<Type> _messageErrorHandlers;
        private static readonly IReadOnlyCollection<Type> _signalHandlers;
        private static readonly IReadOnlyCollection<Type> _signalValidators;

        static EventHubDependencyInjectionExtensions()
        {
            var types = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => (t.IsAssignableTo(typeof(IMessageValidator)) ||
                                             t.IsAssignableTo(typeof(IMessageHandler)) ||
                                             t.IsAssignableTo(typeof(IMessageErrorHandler)) ||
                                             t.IsAssignableTo(typeof(ISignalValidator)) ||
                                             t.IsAssignableTo(typeof(ISignalHandler))) &&
                                             !t.IsInterface && !t.IsAbstract)
                                .ToList();

            _messageValidators = types
                .Where(t => t.IsAssignableTo(typeof(IMessageValidator)))
                .ToList()
                .AsReadOnly();

            _messageHandlers = types
                .Where(t => t.IsAssignableTo(typeof(IMessageHandler)))
                .ToList()
                .AsReadOnly();

            _messageErrorHandlers = types
                .Where(t => t.IsAssignableTo(typeof(IMessageErrorHandler)))
                .ToList()
                .AsReadOnly();

            _signalValidators = types
                .Where(t => t.IsAssignableTo(typeof(ISignalValidator)))
                .ToList()
                .AsReadOnly();

            _signalHandlers = types
                .Where(t => t.IsAssignableTo(typeof(ISignalHandler)))
                .ToList()
                .AsReadOnly();
        }

        public static IServiceCollection AddEventHandler(
            this IServiceCollection services)
        {
            services.AddEventMessageValidators<Message>();
            services.AddEventMessageHandlers<Message>();
            services.AddEventErrorMessageHandlers<Message>();
            services.AddEventSignalsValidators<Signal>();
            services.AddEventSignalHandlers<Signal>();
            services.AddAutoMapper(typeof(AutoMapperMarker));
            services.AddHostedService<ProcessEventHandlerService>();
            return services;
        }

        private static void AddEventMessageValidators<Message>(
            this IServiceCollection services)
        {
            var descriptors = _messageValidators.Select(
                s => ServiceDescriptor.Singleton(typeof(IMessageValidator<Message>), s));
            services.TryAddEnumerable(descriptors);
        }

        private static void AddEventMessageHandlers<Message>(
            this IServiceCollection services)
        {
            var descriptors = _messageHandlers.Select(
                s => ServiceDescriptor.Singleton(typeof(IMessageHandler<Message>), s));
            services.TryAddEnumerable(descriptors);
        }

        private static void AddEventErrorMessageHandlers<Message>(
            this IServiceCollection services)
        {
            var descriptors = _messageErrorHandlers.Select(
                s => ServiceDescriptor.Singleton(typeof(IMessageErrorHandler<Message>), s));
            services.TryAddEnumerable(descriptors);
        }

        private static void AddEventSignalsValidators<Signal>(
            this IServiceCollection services)
        {
            var descriptors = _signalValidators.Select(
                s => ServiceDescriptor.Singleton(typeof(ISignalValidator<Signal>), s));
            services.TryAddEnumerable(descriptors);
        }

        private static void AddEventSignalHandlers<Signal>(
            this IServiceCollection services)
        {
            var descriptors = _signalHandlers.Select(
                s => ServiceDescriptor.Singleton(typeof(ISignalHandler<Signal>), s));
            services.TryAddEnumerable(descriptors);
        }
    }

}
