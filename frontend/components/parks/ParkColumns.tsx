"use client";

import { ColumnDef } from "@tanstack/react-table";

import type { Park } from "@/types/park";

import { AreaBadge } from "@/components/ui/AreaBadge";

export const parkColumns: ColumnDef<Park>[] = [

    {
        accessorKey: "name",
        header: "Park",
    },

    {
        accessorKey: "areaInHectares",
        header: "Area",

        cell: ({ row }) => (

            <AreaBadge
                hectares={
                    row.original.areaInHectares
                }
            />

        ),
    },

];