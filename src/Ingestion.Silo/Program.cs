using ServiceModels;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.UseOrleans((context, silo) =>
{
    silo.ConfigureServices(services =>
    {
        services.AddRabbitMq(cfg =>
        {
            cfg.Host(context.Configuration.GetConnectionString("rabbitmq"));
            cfg.Topology(t =>
            {
                t.Queues.WithFullName();
                t.RoutingKeys.WithPattern("#");
                t.Exchanges.WithFullName();
            });
            cfg.Produce<VehicleState>();
        });
    });

    if (builder.Environment.IsRunningInDocker())
    {
        silo.UseRedisClustering(context.Configuration.GetConnectionString("redis"));
        silo.AddRedisGrainStorageAsDefault(context.Configuration.GetConnectionString("redis"));
    }
    else
    {
        silo.UseLocalhostClustering();
        silo.AddMemoryGrainStorageAsDefault();
    }

    silo.UseDashboard(options => options.Port = 7000);
    silo.ConfigureLogging(options => options.AddConsole());
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Map("/dashboard", x => x.UseOrleansDashboard());

app.Run();
