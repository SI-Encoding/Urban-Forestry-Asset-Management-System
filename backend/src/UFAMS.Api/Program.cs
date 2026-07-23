using UFAMS.Infrastructure;
using UFAMS.Application;
using UFAMS.Api.Middleware;
using UFAMS.Api.Endpoints;
using UFAMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "UFAMS API",
            Version = "v1",
            Description =
                """
                Urban Forest Asset Management System API.

                Supports:
                - Tree asset management
                - Species lookup
                - Park inventory reporting
                - Tree inspections
                - Maintenance work orders
                """
        });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider
        .GetRequiredService<UFAMSDbContext>();

    await db.Database.MigrateAsync();

    await DatabaseInitializer.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapTreeEndpoints();
app.MapSpeciesEndpoints();
app.MapParkEndpoints();
app.MapInspectionEndpoints();
app.MapWorkOrderEndpoints();
app.Run();

public partial class Program
{
}