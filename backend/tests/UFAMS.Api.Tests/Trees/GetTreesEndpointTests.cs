using System.Net.Http.Json;
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
                    JsonOptions);

        trees.Should().NotBeNull();

        trees.Should()
            .NotBeEmpty();
    }
}