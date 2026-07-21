using Microsoft.Extensions.DependencyInjection;
using UFAMS.Application.Features.Parks.GetParks;
using UFAMS.Application.Features.Species.GetSpecies;
using UFAMS.Application.Features.Trees.GetTree;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Features.Trees.RegisterTree;
using UFAMS.Application.Features.Trees.UpdateMeasurements;
using UFAMS.Application.Features.Trees.RelocateTree;
using UFAMS.Application.Features.Trees.UpdateHealth;
using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Features.Inspections.GetTreeInspections;
using UFAMS.Application.Features.Inspections.GetInspection;
using UFAMS.Application.Features.Inspections.UpdateNotes;
using UFAMS.Application.Features.Inspections.UpdateRecommendation;
using UFAMS.Application.Features.Inspections.ScheduleFollowUp;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
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
        services.AddScoped<CreateInspectionHandler>();
        services.AddScoped<GetTreeInspectionsHandler>();
        services.AddScoped<GetInspectionHandler>();
        services.AddScoped<UpdateInspectionNotesHandler>();
        services.AddScoped<UpdateInspectionRecommendationHandler>();
        services.AddScoped<ScheduleFollowUpHandler>();
        services.AddScoped<CreateWorkOrderHandler>();
        return services;
    }
}