using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Persistence.SeedData;

public static class EmployeeSeed
{
    public static IReadOnlyList<Employee> Create() =>
    [
        new(
            "John Smith",
            "Senior Arborist"),

        new(
            "Sarah Johnson",
            "Urban Forestry Technician"),

        new(
            "Michael Chen",
            "Arborist"),

        new(
            "Emily Wilson",
            "Forestry Inspector"),

        new(
            "David Brown",
            "GIS Technician"),

        new(
            "Jennifer Lee",
            "Parks Maintenance Worker"),

        new(
            "Robert Taylor",
            "Crew Supervisor"),

        new(
            "Amanda White",
            "Operations Coordinator"),

        new(
            "Daniel Garcia",
            "Asset Management Analyst"),

        new(
            "Lisa Anderson",
            "Urban Forestry Planner"),

        new(
            "Kevin Martin",
            "Arborist"),

        new(
            "Olivia Thompson",
            "Forestry Technician")
    ];
}