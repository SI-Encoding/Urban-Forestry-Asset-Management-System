namespace UFAMS.Infrastructure.ArcGIS;

public sealed class OAuthState
{
    public required string State { get; init; }

    public required string CodeVerifier { get; init; }

    public DateTime ExpiresAt { get; init; }
}