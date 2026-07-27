export type WorkOrderStatus =
    | "Open"
    | "InProgress"
    | "Completed"
    | "Cancelled";

export interface WorkOrder {
    id: string;
    treeId: string;

    description: string;

    status: WorkOrderStatus;

    createdDate: string;

    dueDate: string;

    completedDate?: string | null;
}