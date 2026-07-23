using System.Net.Http.Json;

namespace UFAMS.Api.Tests.Common;

public abstract class ApiTestBase
{
    protected readonly HttpClient Client;

    protected ApiTestBase(
        CustomWebApplicationFactory factory)
    {
        factory.SeedDatabase();

        Client = factory.CreateClient();
    }
}