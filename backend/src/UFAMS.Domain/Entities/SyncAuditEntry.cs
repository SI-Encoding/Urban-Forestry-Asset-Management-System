using UFAMS.Domain.Enums;

namespace UFAMS.Domain.Entities;

public class SyncAuditEntry
{
public int Id { get; private set; }


public int SyncAuditId { get; private set; }

public SyncAudit SyncAudit { get; private set; } = null!;

public string AssetTag { get; private set; } = string.Empty;

public string Action { get; private set; } = string.Empty;

public string Reason { get; private set; } = string.Empty;

public DateTime CreatedAt { get; private set; }


private SyncAuditEntry()
{
}


public SyncAuditEntry(
    string assetTag,
    string action,
    string reason)
{
    if (string.IsNullOrWhiteSpace(assetTag))
    {
        throw new ArgumentException(
            "Asset tag is required.",
            nameof(assetTag));
    }

    if (string.IsNullOrWhiteSpace(action))
    {
        throw new ArgumentException(
            "Action is required.",
            nameof(action));
    }

    AssetTag = assetTag;

    Action = action;

    Reason = reason;

    CreatedAt = DateTime.UtcNow;
}


}
