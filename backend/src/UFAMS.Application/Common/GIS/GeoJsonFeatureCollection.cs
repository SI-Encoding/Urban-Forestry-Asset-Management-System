namespace UFAMS.Application.Common.GIS;

public sealed record GeoJsonFeatureCollection(
    string Type,
    IReadOnlyList<GeoJsonFeature> Features);