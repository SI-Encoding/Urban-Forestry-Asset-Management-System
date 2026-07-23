using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Common.GIS;

namespace UFAMS.Api.Tests.Trees;

public class ExportTreesGeoJsonEndpointTests
    : ApiTestBase
{
    public ExportTreesGeoJsonEndpointTests(
        CustomWebApplicationFactory factory)
        : base(factory)
    {
    }


    [Fact]
    public async Task ExportTreesGeoJson_ReturnsFeatureCollection()
    {
        var response =
            await Client.GetAsync("/trees/geojson");


        response.EnsureSuccessStatusCode();


        var geoJson =
            await response.Content.ReadFromJsonAsync<
                GeoJsonFeatureCollection>();


        geoJson.Should().NotBeNull();

        geoJson!.Type.Should()
            .Be("FeatureCollection");

        geoJson.Features.Should()
            .NotBeEmpty();
    }
}