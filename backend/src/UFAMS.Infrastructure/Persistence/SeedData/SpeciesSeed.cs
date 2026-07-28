using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Persistence.SeedData;

public static class SpeciesSeed
{
    public static IReadOnlyList<Species> Create() =>
    [
        new Species(
            "Douglas Fir",
            "Pseudotsuga menziesii",
            true),

        new Species(
            "Western Red Cedar",
            "Thuja plicata",
            true),

        new Species(
            "Western Hemlock",
            "Tsuga heterophylla",
            true),

        new Species(
            "Sitka Spruce",
            "Picea sitchensis",
            true),

        new Species(
            "Bigleaf Maple",
            "Acer macrophyllum",
            true),

        new Species(
            "Vine Maple",
            "Acer circinatum",
            true),

        new Species(
            "Pacific Dogwood",
            "Cornus nuttallii",
            true),

        new Species(
            "Garry Oak",
            "Quercus garryana",
            true),

        new Species(
            "Red Maple",
            "Acer rubrum",
            false),

        new Species(
            "Japanese Maple",
            "Acer palmatum",
            false),

        new Species(
            "Cherry Blossom",
            "Prunus serrulata",
            false),

        new Species(
            "London Plane",
            "Platanus × acerifolia",
            false)
    ];
}