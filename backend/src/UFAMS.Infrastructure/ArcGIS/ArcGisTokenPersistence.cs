using System.Text.Json;

namespace UFAMS.Infrastructure.ArcGIS;

public sealed class ArcGisTokenPersistence
{
    private readonly string _filePath =
        Path.Combine(
            AppContext.BaseDirectory,
            "arcgis-token.json");

    public async Task SaveAsync(
        ArcGisUserToken token,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(
            token,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(
            _filePath,
            json,
            cancellationToken);
    }

    public async Task<ArcGisUserToken?> LoadAsync(
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath))
        {
            return null;
        }

        var json =
            await File.ReadAllTextAsync(
                _filePath,
                cancellationToken);

        return JsonSerializer.Deserialize<ArcGisUserToken>(json);
    }

    public void Delete()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}