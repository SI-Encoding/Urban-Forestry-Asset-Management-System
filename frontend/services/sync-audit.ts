import { apiFetch } from "./api-client";

export interface SyncAuditEntry {
    id: number;
    assetTag: string;
    action: string;
    reason: string;
    createdAt: string;
}

export interface SyncAudit {
    id: number;
    startedAt: string;
    completedAt: string | null;
    status: string;
    createdCount: number;
    updatedCount: number;
    failedCount: number;
    ignoredCount: number;
    entries: SyncAuditEntry[];
}

export async function getSyncAudits(): Promise<
    SyncAudit[]
> {
    return apiFetch<SyncAudit[]>(
        "/arcgis/sync/audits"
    );
}
