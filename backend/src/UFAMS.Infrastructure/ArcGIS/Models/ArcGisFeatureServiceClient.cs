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
}