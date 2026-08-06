using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace UFAMS.Infrastructure.ArcGIS;

public static class CoordinateTransformer
{
    private static readonly ICoordinateTransformation Transform;


    static CoordinateTransformer()
    {
        var factory =
            new CoordinateSystemFactory();

        var transformationFactory =
            new CoordinateTransformationFactory();


        // EPSG:3857 Web Mercator
        var webMercator =
            factory.CreateFromWkt(
                """
                PROJCS["WGS_1984_Web_Mercator_Auxiliary_Sphere",
                GEOGCS["GCS_WGS_1984",
                DATUM["D_WGS_1984",
                SPHEROID["WGS_1984",6378137.0,298.257223563]],
                PRIMEM["Greenwich",0.0],
                UNIT["Degree",0.0174532925199433]],
                PROJECTION["Mercator_Auxiliary_Sphere"],
                PARAMETER["False_Easting",0.0],
                PARAMETER["False_Northing",0.0],
                PARAMETER["Central_Meridian",0.0],
                PARAMETER["Standard_Parallel_1",0.0],
                UNIT["Meter",1.0]]
                """);


        // EPSG:4326
        var wgs84 =
            factory.CreateFromWkt(
                """
                GEOGCS["GCS_WGS_1984",
                DATUM["D_WGS_1984",
                SPHEROID["WGS_1984",6378137.0,298.257223563]],
                PRIMEM["Greenwich",0.0],
                UNIT["Degree",0.0174532925199433]]
                """);


        Transform =
            transformationFactory.CreateFromCoordinateSystems(
                webMercator,
                wgs84);
    }


    public static (
        double Latitude,
        double Longitude
    ) Convert(
        double x,
        double y)
    {
        var result =
            Transform.MathTransform.Transform(
                new[]
                {
                    x,
                    y
                });


        return
        (
            Latitude: result[1],
            Longitude: result[0]
        );
    }
}