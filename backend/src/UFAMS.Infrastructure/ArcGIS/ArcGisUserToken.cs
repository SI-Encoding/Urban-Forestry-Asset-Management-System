namespace UFAMS.Infrastructure.ArcGIS;

public sealed class ArcGisUserToken
{
    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public DateTime ExpiresAt { get; init; }
}