export interface Inspection {
    id: string;
    treeId: string;

    inspectionDate: string;

    observedHealth:
        | "Excellent"
        | "Good"
        | "Fair"
        | "Poor"
        | "Dead";

    notes: string;

    recommendation: string;

    nextInspectionDate: string | null;
}