using Microsoft.EntityFrameworkCore;
using SmartFleets.Api;
using SmartFleets.Application.Handlers;
using SmartFleets.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("smartfleets.db"));
});
builder.AddServiceDefaults();

builder.Services.AddRabbitMq();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<HandlerMarker>());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapDefaultEndpoints();

app.Run();
