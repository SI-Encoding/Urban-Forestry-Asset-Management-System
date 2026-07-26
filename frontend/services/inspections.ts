import { apiFetch } from "./api-client";

import type { Inspection } from "@/types/inspection";

export function getTreeInspections(
    treeId: string
) {
    return apiFetch<Inspection[]>(
        `/trees/${treeId}/inspections`
    );
}