using Microsoft.Extensions.DependencyInjection;
using UFAMS.Application.Features.Parks.GetParks;
using UFAMS.Application.Features.Species.GetSpecies;
using UFAMS.Application.Features.Trees.RegisterTree;

namespace UFAMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<RegisterTreeHandler>();

        services.AddScoped<GetSpeciesHandler>();

        services.AddScoped<GetParksHandler>();

        return services;
    }
}