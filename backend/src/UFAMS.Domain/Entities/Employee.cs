using UFAMS.Domain.Common;

namespace UFAMS.Domain.Entities;

public class Employee : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Role { get; private set; } = string.Empty;


    private Employee()
    {
    }


    public Employee(
        string name,
        string role)
    {
        Name = ValidateName(name);
        Role = ValidateRole(role);
    }


    private static string ValidateName(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Employee name is required.",
                nameof(name));
        }

        return name.Trim();
    }


    private static string ValidateRole(
        string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException(
                "Employee role is required.",
                nameof(role));
        }

        return role.Trim();
    }
}