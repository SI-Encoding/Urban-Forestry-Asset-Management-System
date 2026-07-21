namespace UFAMS.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(
        string entityName,
        Guid id)
        : base($"{entityName} '{id}' was not found.")
    {
    }
}