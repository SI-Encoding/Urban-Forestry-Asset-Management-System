import type { ColumnDef } from "@tanstack/react-table";

import type { Inspection } from "@/types/inspection";

import { HealthBadge } from "@/components/ui/HealthBadge";


export const inspectionColumns: ColumnDef<Inspection>[] = [

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
        accessorKey: "inspectionDate",
        header: "Inspection Date",
        cell: ({ row }) =>
            new Date(
                row.original.inspectionDate
            ).toLocaleDateString(),
    },

    {
        accessorKey: "observedHealth",
        header: "Health",
        cell: ({ row }) => (
            <HealthBadge
                status={
                    row.original.observedHealth
                }
            />
        ),
    },

    {
        accessorKey: "recommendation",
        header: "Recommendation",
    },

    {
        accessorKey: "nextInspectionDate",
        header: "Next Inspection",
        cell: ({ row }) => {

            const date =
                row.original.nextInspectionDate;

            if (!date) {
                return "-";
            }

            return new Date(
                date
            ).toLocaleDateString();

        },
    },

];