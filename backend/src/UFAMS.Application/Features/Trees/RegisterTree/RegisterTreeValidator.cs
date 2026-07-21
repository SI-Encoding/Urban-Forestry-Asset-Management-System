namespace UFAMS.Application.Features.Trees.RegisterTree;

public sealed class RegisterTreeValidator
{
    public IReadOnlyList<string> Validate(RegisterTreeCommand command)
    {
        List<string> errors = [];

        if (string.IsNullOrWhiteSpace(command.AssetTag))
            errors.Add("Asset tag is required.");

        if (command.SpeciesId == Guid.Empty)
            errors.Add("Species is required.");

        if (command.ParkId == Guid.Empty)
            errors.Add("Park is required.");

        if (command.PlantingDate > DateOnly.FromDateTime(DateTime.Today))
            errors.Add("Planting date cannot be in the future.");

        if (command.HeightInMeters < 0)
            errors.Add("Height cannot be negative.");

        if (command.DiameterInCentimeters < 0)
            errors.Add("Diameter cannot be negative.");

        return errors;
    }
}