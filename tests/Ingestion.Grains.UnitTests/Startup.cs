using Microsoft.Extensions.DependencyInjection;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Storage;
using Orleans.TestingHost;
using ServiceModels;
using SmartFleets.RabbitMQ.Base;
using System.Reflection;

namespace Ingestion.Grains.UnitTests
{
    public sealed class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(Mock<>));
            services.AddScoped(s => s.GetService<Mock<IBus>>().Object);
            services.AddScoped(s => s.GetService<Mock<IVehicleStateGrain>>().Object);
            services.AddScoped(s => s.GetService<Mock<IVehicleHistoricalStateGrain>>().Object);
            services.AddMockSilo();

            services.AddScoped(s =>
            {
                Mock<IGrainFactory> mock = s.GetService<Mock<IGrainFactory>>();

                mock.Setup(proxy => proxy.GetGrain<IVehicleStateGrain>(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(s.GetService<IVehicleStateGrain>());

                mock.Setup(proxy => proxy.GetGrain<IVehicleHistoricalStateGrain>(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(s.GetService<IVehicleHistoricalStateGrain>());

                return mock.Object;
            });

            services.AddScoped(s =>
            {
                RuntimeContext.SetExecutionContext(s.GetService<IGrainContext>());
                var bus = s.GetService<IBus>();
                var storage = s.GetService<Mock<IStorage<VehicleHistoricalState>>>();
                storage.SetupGet(proxy => proxy.State).Returns(new VehicleHistoricalState());
                var grain = new VehicleHistoricalStateGrain(bus);
                typeof(Grain<VehicleHistoricalState>).GetField("_storage", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(grain, storage.Object);
                return grain;
            });

            services.AddScoped(s =>
            {
                RuntimeContext.SetExecutionContext(s.GetService<IGrainContext>());
                var bus = s.GetService<IBus>();
                var storage = s.GetService<Mock<IStorage<VehicleState>>>();
                storage.SetupGet(proxy => proxy.State).Returns(new VehicleState());
                var grain = new VehicleStateGrain(bus);
                typeof(Grain<VehicleState>).GetField("_storage", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(grain, storage.Object);
                return grain;
            });
        }
    }

    file static class ServicesExtensions
    {
        public static IServiceCollection AddMockSilo(this IServiceCollection services)
        {
            services.AddScoped(s => s.GetService<Mock<IGrainStorage>>().Object);
            services.AddScoped(s =>
            {
                Mock<IGrainRuntime> mock = s.GetService<Mock<IGrainRuntime>>();
                mock.SetupGet(proxy => proxy.GrainFactory).Returns(s.GetService<IGrainFactory>());
                return mock.Object;
            });

            services.AddScoped(s =>
            {
                Mock<IGrainContext> mock = s.GetService<Mock<IGrainContext>>();
                mock.SetupGet(proxy => proxy.GrainInstance).Returns(new object());
                mock.SetupGet(proxy => proxy.ActivationServices).Returns(s);
                mock.SetupGet(proxy => proxy.ObservableLifecycle).Returns(() =>
                {
                    var lifecylcle = s.GetService<Mock<IGrainLifecycle>>();
                    lifecylcle.Setup(proxy => proxy.AddMigrationParticipant(It.IsAny<IGrainMigrationParticipant>()))
                              .Callback((IGrainMigrationParticipant observer) => observer.OnRehydrate(default));

                    return lifecylcle.Object;
                });
                return mock.Object;
            });
            return services;
        }
    }
}

