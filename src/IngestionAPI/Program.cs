var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddRabbitMq()
       .AddRedis()
       .AddAutoMapper(typeof(SignalsMappingProfile));

builder.Build()
       .Run();
