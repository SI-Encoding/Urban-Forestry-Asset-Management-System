using FluentAssertions;
using Moq;
using UFAMS.Application.Common.GIS;
using UFAMS.Application.Features.Trees.ExportTreesGeoJson;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.Trees.ExportTreesGeoJson;

public class ExportTreesGeoJsonHandlerTests
{
    [Fact]
    public async Task Handle_WhenTreesExist_ReturnsFeatureCollection()
    {
        // Arrange
        var trees = new List<Tree>
        {
            TestDataFactory.CreateTree()
        };

        var repository = new Mock<ITreeRepository>();

        repository
            .Setup(r => r.SearchAsync(
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<TreeHealthStatus?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(trees);

        var handler =
            new ExportTreesGeoJsonHandler(repository.Object);

        var query =
            new ExportTreesGeoJsonQuery(
                null,
                null,
                null,
                null,
                null,
                null,
                null);

        // Act
        GeoJsonFeatureCollection result =
            await handler.Handle(query);

        // Assert
        result.Type.Should()
            .Be("FeatureCollection");

        result.Features.Should()
            .HaveCount(1);

        result.Features[0].Type.Should()
            .Be("Feature");
    }

    [Fact]
    public async Task Handle_ExportsLongitudeBeforeLatitude()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree(
                latitude: 49.3043,
                longitude: -123.1443);

        var repository =
            new Mock<ITreeRepository>();

        repository
            .Setup(r => r.SearchAsync(
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<TreeHealthStatus?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Tree> { tree });

        var handler =
            new ExportTreesGeoJsonHandler(repository.Object);

        // Act
        var result =
            await handler.Handle(
                new ExportTreesGeoJsonQuery(
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null));

        // Assert
        result.Features[0]
            .Geometry
            .Coordinates[0]
            .Should()
            .Be(-123.1443);

        result.Features[0]
            .Geometry
            .Coordinates[1]
            .Should()
            .Be(49.3043);
    }
}