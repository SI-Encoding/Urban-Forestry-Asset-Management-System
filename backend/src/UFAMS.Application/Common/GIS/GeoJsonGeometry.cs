namespace UFAMS.Application.Common.GIS;

public sealed record GeoJsonGeometry(
    string Type,
    double[] Coordinates);