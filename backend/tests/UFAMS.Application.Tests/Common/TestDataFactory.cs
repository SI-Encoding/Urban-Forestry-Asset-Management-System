using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Tests.Common;

public static class TestDataFactory
{
    public static Species CreateSpecies(
        string commonName = "Douglas Fir",
        string scientificName = "Pseudotsuga menziesii")
    {
        return new Species(
            commonName,
            scientificName,
            true);
    }

    public static Park CreatePark(
        string name = "Stanley Park")
    {
        return new Park(
            name,
            new GeoCoordinate(
                49.3043,
                -123.1443),
            405);
    }

    public static Tree CreateTree(
        string assetTag = "TREE-001",
        TreeHealthStatus health = TreeHealthStatus.Good,
        string speciesName = "Douglas Fir",
        string parkName = "Stanley Park")
    {
        return new Tree(
            assetTag,
            CreateSpecies(speciesName),
            CreatePark(parkName),
            new GeoCoordinate(
                49.3043,
                -123.1443),
            health,
            new DateOnly(2020, 1, 1),
            12,
            30);
    }

    public static Employee CreateEmployee(
        string name = "John Smith",
        string role = "Arborist")
    {
        return new Employee(
            name,
            role);
    }

    public static Inspection CreateInspection(
        Tree? tree = null)
    {
        return new Inspection(
            tree?.Id ?? Guid.NewGuid(),
            new DateOnly(2026, 1, 1),
            TreeHealthStatus.Good,
            "Initial inspection notes",
            "Continue monitoring",
            null);
    }

    public static WorkOrder CreateWorkOrder(
        Tree? tree = null)
    {
        return new WorkOrder(
            tree ?? CreateTree(),
            "Prune branches",
            null);
    }

    
}