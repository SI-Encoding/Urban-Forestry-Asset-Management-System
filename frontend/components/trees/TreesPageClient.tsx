"use client";

import { useState } from "react";

import type { Tree } from "@/types/tree";

import { EntityTable } from "@/components/ui/EntityTable";
import { treeColumns } from "@/components/trees/TreeColumns";
import { TreeDetailsDrawer } from "@/components/trees/TreeDetailsDrawer";


interface TreesPageClientProps {
    trees: Tree[];
}


export function TreesPageClient({
    trees,
}: TreesPageClientProps) {

    const [selectedTree, setSelectedTree] =
        useState<Tree | null>(null);


    return (
        <>
            <EntityTable
                columns={treeColumns}
                data={trees}
                emptyMessage="No trees found."
                onRowClick={setSelectedTree}
            />


            <TreeDetailsDrawer
                tree={selectedTree}
                open={!!selectedTree}
                onOpenChange={(open) => {
                    if (!open) {
                        setSelectedTree(null);
                    }
                }}
            />
        </>
    );
}