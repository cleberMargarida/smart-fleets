using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedisContainer("redis");
var rabbitmq = builder.AddRabbitMQContainer("rabbitmq");

var mssql = builder.AddSqlServerContainer("mssql")
                   .WithVolumeMount("mssql", "/var/opt/mssql", VolumeMountType.Named)
                   .AddDatabase("smartfleets.db");

builder.AddProject<Ingestion_Api>("ingestion.api")
       .WithReference(rabbitmq)
       .WithReference(redis);

builder.AddProject<Ingestion_Silo>("ingestion.silo")
       .WithReference(rabbitmq)
       .WithReference(redis);

builder.AddProject<SmartFleets_Api>("smartfleets.api")
       .WithReference(mssql)
       .WithReference(rabbitmq);

builder.Build().Run();
