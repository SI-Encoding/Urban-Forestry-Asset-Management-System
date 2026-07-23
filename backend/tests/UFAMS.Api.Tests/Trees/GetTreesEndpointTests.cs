using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Trees.GetTrees;

namespace UFAMS.Api.Tests.Trees;

public class GetTreesEndpointTests
    : ApiTestBase
{
    public GetTreesEndpointTests()
        : base(new CustomWebApplicationFactory())
    {
    }

    [Fact]
    public async Task GetTrees_ReturnsSuccess()
    {
        var response =
            await Client.GetAsync("/trees");

        response.EnsureSuccessStatusCode();

        var trees =
            await response.Content.ReadFromJsonAsync<
                List<GetTreesResponse>>(
                    new JsonSerializerOptions
                    {
                        Converters =
                        {
                            new JsonStringEnumConverter()
                        }
                    });

        trees.Should().NotBeNull();
        trees.Should().NotBeEmpty();
    }
}