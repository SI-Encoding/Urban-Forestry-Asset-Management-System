namespace UFAMS.Application.Features.Trees.UpdateMeasurements;

public sealed record UpdateTreeMeasurementsCommand(
    double HeightInMeters,
    double DiameterInCentimeters);