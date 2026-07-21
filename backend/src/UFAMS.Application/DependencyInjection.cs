using Microsoft.Extensions.DependencyInjection;
using UFAMS.Application.Features.Parks.GetParks;
using UFAMS.Application.Features.Species.GetSpecies;
using UFAMS.Application.Features.Trees.GetTree;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Features.Trees.RegisterTree;
using UFAMS.Application.Features.Trees.UpdateMeasurements;
using UFAMS.Application.Features.Trees.RelocateTree;
using UFAMS.Application.Features.Trees.UpdateHealth;
namespace UFAMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<RegisterTreeHandler>();
        services.AddScoped<GetTreesHandler>();
        services.AddScoped<GetTreeHandler>();
        services.AddScoped<GetSpeciesHandler>();
        services.AddScoped<GetParksHandler>();
        services.AddScoped<UpdateTreeMeasurementsHandler>();
        services.AddScoped<RelocateTreeHandler>();
        services.AddScoped<UpdateTreeHealthHandler>();
        return services;
    }
}