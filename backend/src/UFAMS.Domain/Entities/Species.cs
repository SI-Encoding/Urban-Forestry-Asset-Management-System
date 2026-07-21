namespace UFAMS.Domain.Entities;

using UFAMS.Domain.Common;

public class Species : BaseEntity
{
    public string CommonName { get; private set; }

    public string ScientificName { get; private set; }

    public bool IsNative { get; private set; }

    public string? Description { get; private set; }

    private Species()
    {
        CommonName = null!;
        ScientificName = null!;
    }

    public Species(
        string commonName,
        string scientificName,
        bool isNative,
        string? description = null)
    {
        CommonName = ValidateCommonName(commonName);
        ScientificName = ValidateScientificName(scientificName);

        IsNative = isNative;
        Description = description?.Trim();
    }

    public void UpdateDetails(
        string commonName,
        string scientificName,
        bool isNative,
        string? description)
    {
        CommonName = ValidateCommonName(commonName);
        ScientificName = ValidateScientificName(scientificName);

        IsNative = isNative;
        Description = description?.Trim();

        MarkUpdated();
    }

    private static string ValidateCommonName(string commonName)
    {
        if (string.IsNullOrWhiteSpace(commonName))
            throw new ArgumentException(
                "Common name is required.",
                nameof(commonName));

        return commonName.Trim();
    }

    private static string ValidateScientificName(string scientificName)
    {
        if (string.IsNullOrWhiteSpace(scientificName))
            throw new ArgumentException(
                "Scientific name is required.",
                nameof(scientificName));

        return scientificName.Trim();
    }
}