using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using UFAMS.Infrastructure.Persistence;
using UFAMS.Application.Interfaces;
using UFAMS.Application.AI;

using UFAMS.Infrastructure.Repositories;
using UFAMS.Application.Features.ArcGisSync;

using UFAMS.Infrastructure.ArcGIS;
using UFAMS.Infrastructure.Persistence.Repositories;
using UFAMS.Infrastructure.AI;

namespace UFAMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ------------------------------------------------------------
        // Database
        // ------------------------------------------------------------

        services.AddDbContext<UFAMSDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(
                    "DefaultConnection")));


        // ------------------------------------------------------------
        // Repositories
        // ------------------------------------------------------------

        services.AddScoped<
            ITreeRepository,
            TreeRepository>();

        services.AddScoped<
            ISpeciesRepository,
            SpeciesRepository>();

        services.AddScoped<
            IParkRepository,
            ParkRepository>();

        services.AddScoped<
            IUnitOfWork,
            UnitOfWork>();

        services.AddScoped<
            IInspectionRepository,
            InspectionRepository>();

        services.AddScoped<
            IWorkOrderRepository,
            WorkOrderRepository>();

        services.AddScoped<
            IEmployeeRepository,
            EmployeeRepository>();


        // ------------------------------------------------------------
        // GIS / Synchronization
        // ------------------------------------------------------------

        services.AddScoped<
            SpatialDataSyncService>();

        services.AddSingleton<
            ArcGisTokenStore>();

        services.AddSingleton<
            OAuthStateStore>();

        services.AddSingleton<
            ArcGisTokenPersistence>();

        services.AddScoped<
            ISyncAuditRepository,
            SyncAuditRepository>();


        // ------------------------------------------------------------
        // AI
        // ------------------------------------------------------------

        services.Configure<AiOptions>(
            configuration.GetSection(
                AiOptions.SectionName));

        services.AddHttpClient<
            IAiService,
            GroqAiService>();


        return services;
    }
}