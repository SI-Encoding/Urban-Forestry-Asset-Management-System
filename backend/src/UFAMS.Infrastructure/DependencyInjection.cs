using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UFAMS.Infrastructure.Persistence;
using UFAMS.Application.Interfaces;
using UFAMS.Infrastructure.Repositories;

namespace UFAMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<UFAMSDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITreeRepository, TreeRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IParkRepository, ParkRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IInspectionRepository, InspectionRepository>();
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        return services;
    }
}