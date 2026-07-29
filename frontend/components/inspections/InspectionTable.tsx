"use client";

import { EntityTable } from "@/components/ui/EntityTable";
import { inspectionColumns } from "./InspectionColumns";
import type { Inspection } from "@/types/inspection";


interface Props {
    data: Inspection[];
}


export function InspectionTable({
    data,
}: Props) {

    return (
        <EntityTable
            columns={inspectionColumns}
            data={data}
            emptyMessage="No inspections found."
        />
    );
}