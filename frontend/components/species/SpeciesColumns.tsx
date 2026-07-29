"use client";

import { ColumnDef } from "@tanstack/react-table";

import type { Species } from "@/types/species";

import { NativeBadge } from "@/components/ui/NativeBadge";

export const speciesColumns: ColumnDef<Species>[] = [

    {
        accessorKey: "commonName",
        header: "Common Name",
    },

    {
        accessorKey: "scientificName",
        header: "Scientific Name",

        cell: ({ row }) => (

            <span className="italic">
                {row.original.scientificName}
            </span>

        ),
    },

    {
        accessorKey: "isNative",
        header: "Origin",

        cell: ({ row }) => (

            <NativeBadge
                isNative={
                    row.original.isNative
                }
            />

        ),
    },

];