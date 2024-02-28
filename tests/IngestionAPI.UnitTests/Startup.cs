using IngestionAPI.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartFleet.RabbitMQ.Base;

namespace IngestionAPI.UnitTests
{
    public class Startup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(Mock<>));
            services.AddAutoMapper(typeof(SignalsMappingProfile));
            services.AddScoped(s => s.GetService<Mock<IBus>>().Object);
            services.AddScoped<MessageConsumer>();
        }
    }
}
