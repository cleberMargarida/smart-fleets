using ServiceModels;

var builder = WebApplication.CreateBuilder(args);

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

    silo.UseDashboard(x => x.HostSelf = true);
    silo.ConfigureLogging(options => options.AddConsole());
});

var app = builder.Build();
app.Map("/dashboard", x => x.UseOrleansDashboard());
app.Run();
