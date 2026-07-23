using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UFAMS.Api.Tests.Common;

public abstract class ApiTestBase
{
    protected readonly HttpClient Client;

    protected static readonly JsonSerializerOptions JsonOptions =
        new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };


    protected ApiTestBase(
        CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();

        factory.SeedDatabase();
    }
}