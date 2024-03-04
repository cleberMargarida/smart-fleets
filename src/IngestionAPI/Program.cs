var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleansClient();

builder.Services.AddAutoMapper(typeof(SignalMappingProfile));
builder.Services.AddRabbitMq();
builder.Services.AddRedis();
builder.Services.Configure<PublishHandlerConfiguration>(c =>
{
    c.Timeout = TimeSpan.FromSeconds(30);
    c.BatchSize = 100;
});

builder.Services.AddPipeline(pipeline =>
    pipeline
        .Add<EnrichHandler>()
        .Add<PublishHandler>()
        .Add<VehicleStateHandler>());

var app = builder.Build();

app.Run();
