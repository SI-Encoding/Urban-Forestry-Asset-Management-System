using System.Net.Http.Json;

using Microsoft.Extensions.Options;

using UFAMS.Infrastructure.Configuration;

namespace UFAMS.Infrastructure.ArcGIS;

public sealed class ArcGisAuthenticationService
    : IArcGisAuthenticationService
{
    private readonly HttpClient _httpClient;

    private readonly ArcGisOptions _options;

    public ArcGisAuthenticationService(
        HttpClient httpClient,
        IOptions<ArcGisOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        var form =
            new Dictionary<string, string>
            {
                ["client_id"] =
                    _options.ClientId,

                ["client_secret"] =
                    _options.ClientSecret,

                ["grant_type"] =
                    "client_credentials"
            };

        var response =
            await _httpClient.PostAsync(
                "https://www.arcgis.com/sharing/rest/oauth2/token",
                new FormUrlEncodedContent(form),
                cancellationToken);

        response.EnsureSuccessStatusCode();

        var token =
            await response.Content.ReadFromJsonAsync
            <ArcGisTokenResponse>(
                cancellationToken: cancellationToken);

        if (token is null)
        {
            throw new InvalidOperationException(
                "Unable to retrieve ArcGIS access token.");
        }

        return token.AccessToken;
    }
}