namespace UFAMS.Application.Common.GIS;

public sealed record GeoJsonFeature(
    string Type,
    GeoJsonGeometry Geometry,
    object Properties);