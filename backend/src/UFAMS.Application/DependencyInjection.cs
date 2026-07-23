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
using UFAMS.Application.Features.WorkOrders.GetTreeWorkOrders;
using UFAMS.Application.Features.WorkOrders.AssignWorkOrder;
using UFAMS.Application.Features.WorkOrders.GetWorkOrder;
using UFAMS.Application.Features.WorkOrders.StartWorkOrder;
using UFAMS.Application.Features.WorkOrders.CompleteWorkOrder;
using UFAMS.Application.Features.WorkOrders.CancelWorkOrder;
using UFAMS.Application.Features.Trees.SearchTrees;
using UFAMS.Application.Features.Trees.ExportTreesGeoJson;
using UFAMS.Application.Features.Trees.FindNearbyTrees;
using UFAMS.Application.Features.Parks.GetParkInventory;
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
        services.AddScoped<GetTreeWorkOrdersHandler>();
        services.AddScoped<AssignWorkOrderHandler>();
        services.AddScoped<GetWorkOrderHandler>();
        services.AddScoped<StartWorkOrderHandler>();
        services.AddScoped<CompleteWorkOrderHandler>();
        services.AddScoped<CancelWorkOrderHandler>();
        services.AddScoped<SearchTreesHandler>();
        services.AddScoped<ExportTreesGeoJsonHandler>();
        services.AddScoped<FindNearbyTreesHandler>();
        services.AddScoped<GetParkInventoryHandler>();
        return services;
    }
}