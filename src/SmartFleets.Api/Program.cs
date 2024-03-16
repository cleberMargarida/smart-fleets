using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using SmartFleets.Api;
using SmartFleets.Application;
using SmartFleets.Domain.Repositories;
using SmartFleets.Infrastructure.Data;
using SmartFleets.Infrastructure.Repositories;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

MappingConfiguration.Apply();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    string presentationDocumentationFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    string presentationDocumentationFilePath = Path.Combine(AppContext.BaseDirectory, presentationDocumentationFile);
    c.IncludeXmlComments(presentationDocumentationFilePath);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
});

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddDbContextMigrator();
builder.Services.AddRabbitMq();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFaultRepository, FaultRepository>();
builder.Services.AddScoped<IFaultMetadataRepository, FaultMetadataRepository>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyReference>());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssemblyContaining<AssemblyReference>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
