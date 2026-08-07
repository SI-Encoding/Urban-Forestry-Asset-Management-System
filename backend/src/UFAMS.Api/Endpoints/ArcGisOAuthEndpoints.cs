using UFAMS.Infrastructure.ArcGIS;

namespace UFAMS.Api.Endpoints;


public static class ArcGisOAuthEndpoints
{
    public static void MapArcGisOAuthEndpoints(
        this WebApplication app)
    {

        app.MapGet(
            "/api/arcgis/auth/login",
            (
                ArcGisOAuthService oauth
            ) =>
            {
                var url =
                    oauth.CreateAuthorizationUrl();

                return Results.Redirect(url);
            });



        app.MapGet(
            "/api/arcgis/auth/callback",
            async (
                string code,
                string state,
                OAuthStateStore store,
                ArcGisOAuthService oauth,
                ArcGisTokenStore tokenStore,
                ArcGisTokenPersistence persistence
            ) =>
            {
                var savedState =
                    store.Take(state);

                if (savedState is null)
                {
                    return Results.BadRequest(
                        "Invalid OAuth state.");
                }

                var token =
                    await oauth.ExchangeCodeAsync(
                        code,
                        savedState.CodeVerifier);

                if (token is null)
                {
                    return Results.BadRequest(
                        "Token exchange failed.");
                }

                var userToken =
                    new ArcGisUserToken
                    {
                        AccessToken =
                            token.AccessToken,

                        RefreshToken =
                            token.RefreshToken ?? string.Empty,

                        ExpiresAt =
                            DateTime.UtcNow
                                .AddSeconds(token.ExpiresIn)
                    };

                tokenStore.Save(userToken);

                await persistence.SaveAsync(userToken);

                return Results.Redirect(
                    "http://localhost:3000/arcgis-sync");
            });

            app.MapGet(
            "/api/arcgis/auth/status",
            (ArcGisTokenStore tokenStore) =>
            {
                return Results.Ok(new
                {
                    authenticated = tokenStore.Get() is not null
                });
            });


    }
}