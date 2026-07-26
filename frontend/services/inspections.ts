import { apiFetch } from "./api-client";

import type { Inspection } from "@/types/inspection";

export interface CreateInspectionRequest {
    inspectionDate: string;
    observedHealth: string;
    notes: string;
    recommendation: string;
    nextInspectionDate?: string;
}


export function getTreeInspections(
    treeId: string
) {
    return apiFetch<Inspection[]>(
        `/trees/${treeId}/inspections`
    );
}


export function createInspection(
    treeId: string,
    data: CreateInspectionRequest
) {
    return apiFetch<Inspection>(
        `/trees/${treeId}/inspections`,
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export function updateInspectionNotes(
    id: string,
    notes: string
) {
    return apiFetch<Inspection>(
        `/inspections/${id}/notes`,
        {
            method: "PUT",
            body: JSON.stringify({
                notes,
            }),
        }
    );
}



export function updateInspectionRecommendation(
    id: string,
    recommendation: string
) {
    return apiFetch<Inspection>(
        `/inspections/${id}/recommendation`,
        {
            method: "PUT",
            body: JSON.stringify({
                recommendation,
            }),
        }
    );
}



export function scheduleFollowUp(
    id: string,
    nextInspectionDate?: string
) {
    return apiFetch<Inspection>(
        `/inspections/${id}/follow-up`,
        {
            method: "PUT",
            body: JSON.stringify({
                nextInspectionDate,
            }),
        }
    );
}