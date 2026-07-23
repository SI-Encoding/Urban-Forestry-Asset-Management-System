using System.Text.Json;
using System.Text.Json.Serialization;

namespace UFAMS.Api.Tests.Common;

public abstract class ApiTestBase
{
    protected HttpClient Client { get; }

    protected JsonSerializerOptions JsonOptions { get; }

    protected ApiTestBase(
        CustomWebApplicationFactory factory)
    {
        factory.SeedDatabase();

        Client = factory.CreateClient();

        JsonOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
    }
}