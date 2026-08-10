using UFAMS.Domain.Enums;

namespace UFAMS.Domain.Entities;

public class SyncAudit
{
public int Id { get; private set; }


public DateTime StartedAt { get; private set; }

public DateTime? CompletedAt { get; private set; }

public SyncAuditStatus Status { get; private set; }

public int CreatedCount { get; private set; }

public int UpdatedCount { get; private set; }

public int FailedCount { get; private set; }

public int IgnoredCount { get; private set; }


private readonly List<SyncAuditEntry> _entries = new();

public IReadOnlyCollection<SyncAuditEntry> Entries =>
    _entries.AsReadOnly();


private SyncAudit()
{
}


public SyncAudit(
    DateTime startedAt)
{
    StartedAt = startedAt;
    Status = SyncAuditStatus.Started;
}


public void Complete(
    int createdCount,
    int updatedCount,
    int failedCount,
    int ignoredCount)
{
    CreatedCount = createdCount;
    UpdatedCount = updatedCount;
    FailedCount = failedCount;
    IgnoredCount = ignoredCount;

    CompletedAt = DateTime.UtcNow;

    Status = SyncAuditStatus.Completed;
}


public void MarkFailed()
{
    CompletedAt = DateTime.UtcNow;

    Status = SyncAuditStatus.Failed;
}


public void AddEntry(
    SyncAuditEntry entry)
{
    _entries.Add(entry);
}


}
