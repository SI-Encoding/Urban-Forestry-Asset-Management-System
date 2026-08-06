namespace UFAMS.Infrastructure.ArcGIS;

public sealed class ArcGisTokenStore
{
    private ArcGisUserToken? _token;


    public void Save(
        ArcGisUserToken token)
    {
        _token = token;
    }


    public ArcGisUserToken? Get()
    {
        return _token;
    }


    public void Clear()
    {
        _token = null;
    }
}