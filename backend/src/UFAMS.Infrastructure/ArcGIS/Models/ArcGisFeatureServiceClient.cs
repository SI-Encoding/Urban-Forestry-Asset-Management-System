using System.Net.Http.Json;

using Microsoft.Extensions.Options;

using UFAMS.Infrastructure.ArcGIS.Models;
using UFAMS.Infrastructure.Configuration;

namespace UFAMS.Infrastructure.ArcGIS;

public sealed class ArcGisFeatureServiceClient
    : IArcGisFeatureServiceClient
{
    private readonly HttpClient _httpClient;

    private readonly ArcGisOptions _options;

    private readonly IArcGisAuthenticationService _authenticationService;


    public ArcGisFeatureServiceClient(
        HttpClient httpClient,
        IOptions<ArcGisOptions> options,
        IArcGisAuthenticationService authenticationService)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _authenticationService = authenticationService;
    }


    public async Task<ArcGisFeatureServiceInfo> GetServiceInfoAsync(
        CancellationToken cancellationToken = default)
    {
        var token =
            await _authenticationService.GetAccessTokenAsync(
                cancellationToken);


        var url =
            $"{_options.FeatureServiceUrl}" +
            $"?f=json" +
            $"&token={token}";


        var response =
            await _httpClient.GetAsync(
                url,
                cancellationToken);


        response.EnsureSuccessStatusCode();


        var result =
            await response.Content.ReadFromJsonAsync
            <ArcGisFeatureServiceInfo>(
                cancellationToken: cancellationToken);


        if (result is null)
        {
            throw new InvalidOperationException(
                "Unable to read ArcGIS Feature Service metadata.");
        }


        return result;
    }

    public async Task<IReadOnlyList<ArcGisFeature>> GetFeaturesAsync(
        CancellationToken cancellationToken = default)
    {
        var token =
            await _authenticationService.GetAccessTokenAsync(
                cancellationToken);

        var url =
            $"{_options.FeatureServiceUrl}/0/query" +
            "?where=1%3D1" +
            "&outFields=*" +
            "&returnGeometry=true" +
            "&f=json" +
            $"&token={token}";

        var response =
            await _httpClient.GetAsync(
                url,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<ArcGisQueryResponse>(
                cancellationToken: cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException(
                "Unable to retrieve ArcGIS features.");
        }

        return result.Features
            .Select(feature =>
                new ArcGisFeature(
                    Id: feature.Attributes.OBJECTID.ToString(),
                    AssetTag: feature.Attributes.assetTag,
                    Species: feature.Attributes.species,
                    Park: feature.Attributes.park,
                    HealthStatus: feature.Attributes.healthStatus,
                    Latitude: feature.Geometry.y,
                    Longitude: feature.Geometry.x))
            .ToList();
    }



    public async Task<string> GetRawFeaturesAsync(
    CancellationToken cancellationToken = default)
{
    var token =
        await _authenticationService.GetAccessTokenAsync(
            cancellationToken);

    var url =
        $"{_options.FeatureServiceUrl}/query" +
        "?where=1%3D1" +
        "&outFields=*" +
        "&returnGeometry=true" +
        "&f=pjson" +
        $"&token={token}";

    Console.WriteLine();
    Console.WriteLine("===== ARC GIS QUERY =====");
    Console.WriteLine(url);
    Console.WriteLine("=========================");
    Console.WriteLine();

    var response =
        await _httpClient.GetAsync(
            url,
            cancellationToken);

    var content =
        await response.Content.ReadAsStringAsync(
            cancellationToken);

    Console.WriteLine();
    Console.WriteLine("===== ARC GIS RESPONSE =====");
    Console.WriteLine(content);
    Console.WriteLine("============================");
    Console.WriteLine();

    return content;
}
}