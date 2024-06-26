using AutoMapper;
using Ingestion.Api.Consumers;
using Ingestion.Api.Handlers;
using Ingestion.Api.Handlers.Abstractions;
using Ingestion.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using SmartFleets.RabbitMQ.Base;

namespace Ingestion.Api.UnitTests
{
    public class Startup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(Mock<>));
            services.AddLogging();
            services.AddAutoMapper(typeof(SignalMappingProfile));
            services.AddScoped(s => s.GetRequiredService<Mock<IPipeline>>().Object);
            services.AddScoped<MessageConsumer>();
            services.AddScoped<EnrichHandler>();
            services.AddScoped(s => s.GetRequiredService<Mock<IValidatorHandler>>().Object);
            services.AddScoped(s => s.GetRequiredService<Mock<IBlockingHandler>>().Object);
            services.AddScoped(s => s.GetRequiredService<Mock<IAsyncHandler>>().Object);
            services.AddScoped<IHandler>(s => s.GetRequiredService<Mock<IValidatorHandler>>().Object);
            services.AddScoped<IHandler>(s => s.GetRequiredService<Mock<IBlockingHandler>>().Object);
            services.AddScoped<IHandler>(s => s.GetRequiredService<Mock<IAsyncHandler>>().Object);
            services.AddScoped<Pipeline>();
            services.AddScoped(s => s.GetRequiredService<Mock<IBus>>().Object);
            services.AddScoped(s =>
            {
                var mock = s.GetRequiredService<Mock<IOptions<PublishHandlerConfiguration>>>();
                mock.SetupGet(proxy => proxy.Value).Returns(new PublishHandlerConfiguration());
                return mock.Object;
            });
            services.AddScoped<PublishHandler>();
        }
    }
}

