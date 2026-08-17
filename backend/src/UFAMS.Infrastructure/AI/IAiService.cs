namespace UFAMS.Application.AI;

public interface IAiService
{
    Task<string> GenerateTreeSummaryAsync(
        string prompt,
        CancellationToken cancellationToken = default);
}