using SmartFleets.Api;
using SmartFleets.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRabbitMq();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<HandlerMarker>());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
