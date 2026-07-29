"use client";

import type { ColumnDef } from "@tanstack/react-table";

import type {
    WorkOrder
} from "@/types/workOrder";


import { WorkOrderStatusBadge } from "@/components/ui/WorkOrderStatusBadge";


export const workOrderColumns:
    ColumnDef<WorkOrder>[] = [

    {
        accessorKey: "assetTag",
        header: "Asset",
    },


    {
        accessorKey: "speciesName",
        header: "Species",
    },


    {
        accessorKey: "parkName",
        header: "Park",
    },


    {
        accessorKey: "assignedEmployeeName",
        header: "Assigned To",

        cell: ({
            row
        }) =>
            row.original.assignedEmployeeName
                ?? "Unassigned",
    },


    {
        accessorKey: "description",
        header: "Description",
    },


    {
        accessorKey: "status",
        header: "Status",

        cell: ({
            row
        }) => (

            <WorkOrderStatusBadge
                status={
                    row.original.status
                }
            />

        ),
    },


    {
        accessorKey: "createdDate",
        header: "Created",

        cell: ({
            row
        }) =>
            new Date(
                row.original.createdDate
            ).toLocaleDateString(),
    },


    {
        accessorKey: "dueDate",
        header: "Due Date",

        cell: ({
            row
        }) =>
            row.original.dueDate
                ? new Date(
                    row.original.dueDate
                ).toLocaleDateString()
                : "-",
    },


];