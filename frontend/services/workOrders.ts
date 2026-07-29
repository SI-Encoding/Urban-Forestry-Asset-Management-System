import { apiFetch, apiPost, apiPut } from "./api-client";

import type { WorkOrder } from "@/types/workOrder";


export interface CreateWorkOrderCommand {
    description: string;
    dueDate?: string;
}


export function getWorkOrders() {

    return apiFetch<WorkOrder[]>(
        "/work-orders"
    );
}


export function getTreeWorkOrders(
    treeId: string
) {
    return apiFetch<WorkOrder[]>(
        `/trees/${treeId}/work-orders`
    );
}


export function createWorkOrder(
    treeId: string,
    command: CreateWorkOrderCommand
) {
    return apiPost<WorkOrder>(
        `/trees/${treeId}/work-orders`,
        command
    );
}


export function startWorkOrder(
    id: string
) {
    return apiPut(
        `/work-orders/${id}/start`
    );
}


export function completeWorkOrder(
    id: string
) {
    return apiPut(
        `/work-orders/${id}/complete`
    );
}


export function cancelWorkOrder(
    id: string
) {
    return apiPut(
        `/work-orders/${id}/cancel`
    );
}