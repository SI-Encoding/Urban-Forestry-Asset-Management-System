using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Trees.RegisterTree;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Tests.Features.Trees.RegisterTree;

public class RegisterTreeHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_RegistersTree()
    {
        // Arrange
        var species =
            TestDataFactory.CreateSpecies();

        var park =
            TestDataFactory.CreatePark();

        var treeRepository =
            new Mock<ITreeRepository>();

        var speciesRepository =
            new Mock<ISpeciesRepository>();

        var parkRepository =
            new Mock<IParkRepository>();

        var unitOfWork =
            new Mock<IUnitOfWork>();

        treeRepository
            .Setup(r => r.ExistsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        speciesRepository
            .Setup(r => r.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(species);

        parkRepository
            .Setup(r => r.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(park);

        var handler =
            new RegisterTreeHandler(
                treeRepository.Object,
                speciesRepository.Object,
                parkRepository.Object,
                unitOfWork.Object);

        var command =
            new RegisterTreeCommand(
                "TREE-100",
                species.Id,
                park.Id,
                new GeoCoordinate(
                    49.2827,
                    -123.1207),
                new DateOnly(2020, 1, 1),
                10,
                25);

        // Act
        var result =
            await handler.Handle(command);

        // Assert
        result.AssetTag.Should().Be("TREE-100");
        result.SpeciesName.Should().Be(species.CommonName);
        result.ParkName.Should().Be(park.Name);
        result.HealthStatus.Should().Be(UFAMS.Domain.Enums.TreeHealthStatus.Good);

        treeRepository.Verify(r =>
            r.AddAsync(
                It.IsAny<Tree>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}