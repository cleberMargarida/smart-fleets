var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIngestionEventHub().AddRabbitMQ().AddRedis();
builder.Build().Run();
