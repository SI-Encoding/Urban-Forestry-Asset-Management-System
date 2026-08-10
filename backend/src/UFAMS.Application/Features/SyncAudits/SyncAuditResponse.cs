namespace UFAMS.Application.Features.SyncAudits;

public sealed record SyncAuditResponse(
    int Id,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string Status,
    int CreatedCount,
    int UpdatedCount,
    int FailedCount,
    int IgnoredCount,
    IReadOnlyList<SyncAuditEntryResponse> Entries);

public sealed record SyncAuditEntryResponse(
    int Id,
    string AssetTag,
    string Action,
    string Reason,
    DateTime CreatedAt);