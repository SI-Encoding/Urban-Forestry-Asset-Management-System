"use client";

import {
    useSearchParams
} from "next/navigation";

import {
    useState
} from "react";


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


    const searchParams =
        useSearchParams();


    const healthFilter =
        searchParams.get(
            "health"
        );


    const filteredTrees =

    healthFilter === "attention"

    ?

    trees.filter(

        tree =>

            [
                "Fair",
                "Poor",
                "Dead",
            ].includes(
                tree.healthStatus
            )

    )

    :

    healthFilter

    ?

    trees.filter(

        tree =>

            tree.healthStatus === healthFilter

    )

    :

    trees;



    const [selectedTree, setSelectedTree] =
        useState<Tree | null>(null);



    return (

        <>

            <EntityTable

                columns={treeColumns}

                data={filteredTrees}

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