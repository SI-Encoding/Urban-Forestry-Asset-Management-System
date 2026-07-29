"use client";

import { ColumnDef } from "@tanstack/react-table";
import { Tree } from "@/types/tree";
import { HealthBadge } from "@/components/ui/HealthBadge";

export const treeColumns: ColumnDef<Tree>[] = [
    {
        accessorKey: "assetTag",
        header: "Asset Tag",
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
        accessorKey: "healthStatus",
        header: "Health",
        cell: ({ row }) => (
            <HealthBadge
                status={row.original.healthStatus}
            />
        ),
    },
];