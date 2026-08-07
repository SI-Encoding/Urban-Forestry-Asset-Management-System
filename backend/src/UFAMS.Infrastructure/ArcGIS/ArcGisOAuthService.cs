using Microsoft.Extensions.Options;
using System.Web;
using System.Net.Http.Json;
namespace UFAMS.Infrastructure.ArcGIS;

public class ArcGisOAuthService
{
    private readonly ArcGisOAuthOptions _options;
    private readonly OAuthStateStore _stateStore;

    public ArcGisOAuthService(
        IOptions<ArcGisOAuthOptions> options,
        OAuthStateStore stateStore)
    {
        _options = options.Value;
        _stateStore = stateStore;
    }


    public string CreateAuthorizationUrl()
    {
        var state =
            Guid.NewGuid()
            .ToString("N");


        var verifier =
            PkceGenerator.GenerateCodeVerifier();


        var challenge =
            PkceGenerator.GenerateCodeChallenge(verifier);


        _stateStore.Save(
            new OAuthState
            {
                State = state,
                CodeVerifier = verifier,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            });


        var query =
            HttpUtility.ParseQueryString(string.Empty);


        query["client_id"] =
            _options.ClientId;

        query["response_type"] =
            "code";

        query["redirect_uri"] =
            _options.RedirectUri;

        query["state"] =
            state;

        query["code_challenge"] =
            challenge;

        query["code_challenge_method"] =
            "S256";


        return
            $"{_options.PortalUrl}/sharing/rest/oauth2/authorize?{query}";
    }

    public async Task<ArcGisTokenResponse?> ExchangeCodeAsync(
    string code,
    string codeVerifier)
{
    using var client = new HttpClient();

    var form = new Dictionary<string, string>
    {
        ["client_id"] =
            _options.ClientId,

        ["grant_type"] =
            "authorization_code",

        ["code"] =
            code,

        ["redirect_uri"] =
            _options.RedirectUri,

        ["code_verifier"] =
            codeVerifier,

        ["f"] =
            "json"
    };


    var response =
        await client.PostAsync(
            $"{_options.PortalUrl}/sharing/rest/oauth2/token",
            new FormUrlEncodedContent(form));


    var token =
        await response.Content
            .ReadFromJsonAsync<ArcGisTokenResponse>();


    return token;
}

public async Task<ArcGisTokenResponse?> RefreshTokenAsync(
    string refreshToken)
{
    using var client = new HttpClient();

    var form = new Dictionary<string, string>
    {
        ["client_id"] = _options.ClientId,

        ["grant_type"] = "refresh_token",

        ["refresh_token"] = refreshToken,

        ["f"] = "json"
    };

    var response =
        await client.PostAsync(
            $"{_options.PortalUrl}/sharing/rest/oauth2/token",
            new FormUrlEncodedContent(form));

    response.EnsureSuccessStatusCode();

    return await response.Content.ReadFromJsonAsync<ArcGisTokenResponse>();
}
}