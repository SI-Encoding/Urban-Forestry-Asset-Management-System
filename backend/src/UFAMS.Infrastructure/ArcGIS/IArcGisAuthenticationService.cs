namespace UFAMS.Infrastructure.ArcGIS;

public interface IArcGisAuthenticationService
{
    Task<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default);
}