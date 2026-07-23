using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Species.GetSpecies;

namespace UFAMS.Api.Tests.Species;

public class GetSpeciesEndpointTests
    : ApiTestBase
{
    public GetSpeciesEndpointTests(
        CustomWebApplicationFactory factory)
        : base(factory)
    {
        factory.SeedDatabase();
    }


    [Fact]
    public async Task GetSpecies_ReturnsSuccess()
    {
        // Act
        var response =
            await Client.GetAsync("/species");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var species =
            await response.Content.ReadFromJsonAsync<
                List<GetSpeciesResponse>>();


        species.Should().NotBeNull();
        species.Should().NotBeEmpty();
    }


    [Fact]
    public async Task GetSpecies_ReturnsExpectedSpecies()
    {
        // Act
        var species =
            await Client.GetFromJsonAsync<
                List<GetSpeciesResponse>>(
                    "/species");


        // Assert
        species.Should().NotBeNull();


        species!
            .Should()
            .Contain(s =>
                s.CommonName == "Douglas Fir" &&
                s.ScientificName == "Pseudotsuga menziesii" &&
                s.IsNative);
    }
}