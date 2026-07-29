export type WorkOrderStatus =
    | "Open"
    | "Assigned"
    | "InProgress"
    | "Completed"
    | "Cancelled";


export interface WorkOrder {

    id: string;

    treeId: string;

    assetTag: string;

    speciesName: string;

    parkName: string;


    assignedEmployeeId:
        string | null;

    assignedEmployeeName:
        string | null;


    description: string;


    status: WorkOrderStatus;


    createdDate: string;


    dueDate:
        string | null;


    completedDate:
        string | null;

}