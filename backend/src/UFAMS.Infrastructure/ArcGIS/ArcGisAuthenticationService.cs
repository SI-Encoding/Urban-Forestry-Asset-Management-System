using System.Net.Http.Json;

using Microsoft.Extensions.Options;

using UFAMS.Infrastructure.Configuration;

namespace UFAMS.Infrastructure.ArcGIS;

public sealed class ArcGisAuthenticationService
    : IArcGisAuthenticationService
{
    private readonly HttpClient _httpClient;

    private readonly ArcGisTokenStore _tokenStore;

    private readonly ArcGisOAuthOptions _options;

    public ArcGisAuthenticationService(
        HttpClient httpClient,
        ArcGisTokenStore tokenStore,
        IOptions<ArcGisOAuthOptions> options)
    {
        _httpClient = httpClient;
        _tokenStore = tokenStore;
        _options = options.Value;
    }

    public async Task<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        var token =
            _tokenStore.Get();


        if (token is null)
        {
            throw new InvalidOperationException(
                "ArcGIS user has not authenticated.");
        }


        if (token.ExpiresAt > DateTime.UtcNow)
        {
            return token.AccessToken;
        }


        return await RefreshTokenAsync(
            token.RefreshToken,
            cancellationToken);
    }

    private async Task<string> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var form =
            new Dictionary<string,string>
            {
                ["client_id"] =
                    _options.ClientId,

                ["grant_type"] =
                    "refresh_token",

                ["refresh_token"] =
                    refreshToken,

                ["f"] =
                    "json"
            };


        var response =
            await _httpClient.PostAsync(
                $"{_options.PortalUrl}/sharing/rest/oauth2/token",
                new FormUrlEncodedContent(form),
                cancellationToken);


        response.EnsureSuccessStatusCode();


        var token =
            await response.Content
                .ReadFromJsonAsync<ArcGisTokenResponse>(
                    cancellationToken:
                        cancellationToken);


        if (token is null)
        {
            throw new InvalidOperationException(
                "Unable to refresh ArcGIS token.");
        }


        var updatedToken =
            new ArcGisUserToken
            {
                AccessToken =
                    token.AccessToken,

                RefreshToken =
                    token.RefreshToken ?? refreshToken,

                ExpiresAt =
                    DateTime.UtcNow
                        .AddSeconds(token.ExpiresIn)
            };


        _tokenStore.Save(updatedToken);


        return updatedToken.AccessToken;
    }
}