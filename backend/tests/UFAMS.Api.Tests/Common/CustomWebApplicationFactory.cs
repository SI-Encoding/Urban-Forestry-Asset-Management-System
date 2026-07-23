using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Api.Tests.Common;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(
                    d =>
                    d.ServiceType ==
                    typeof(DbContextOptions<UFAMSDbContext>));


            if (descriptor != null)
            {
                services.Remove(descriptor);
            }


            services.AddDbContext<UFAMSDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase(
                        "UFAMS_Test_DB");
                });
        });
    }


    public void SeedDatabase()
    {
        using var scope =
            Services.CreateScope();


        var db =
            scope.ServiceProvider
                .GetRequiredService<UFAMSDbContext>();


        db.Database.EnsureCreated();


        DatabaseInitializer.SeedAsync(db)
            .GetAwaiter()
            .GetResult();
    }
}