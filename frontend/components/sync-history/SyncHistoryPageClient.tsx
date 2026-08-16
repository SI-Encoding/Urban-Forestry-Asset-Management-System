"use client";

import {
    useMemo,
} from "react";

import {
    useRouter,
} from "next/navigation";

import type {
    ColumnDef,
} from "@tanstack/react-table";

import type {
    SyncAudit,
} from "@/services/sync-audit";

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

import {
    EntityTable,
} from "@/components/ui/EntityTable";


interface SyncHistoryPageClientProps {

    audits: SyncAudit[];

}


export function SyncHistoryPageClient({

    audits,

}: SyncHistoryPageClientProps) {


    const router =
        useRouter();


    const columns =
        useMemo<ColumnDef<SyncAudit>[]>(

            () => [

                {
                    accessorKey: "startedAt",

                    header: "Date",

                    cell: ({ row }) => {

                        const date =
                            new Date(
                                row.original.startedAt
                            );


                        return date.toLocaleString(

                            "en-US",

                            {
                                year: "numeric",
                                month: "numeric",
                                day: "numeric",
                                hour: "numeric",
                                minute: "2-digit",
                                second: "2-digit",
                                hour12: true,
                            }

                        );

                    },

                },


                {
                    accessorKey: "status",

                    header: "Status",

                },


                {
                    accessorKey: "createdCount",

                    header: "Created",

                    cell: ({ row }) => (

                        <div className="text-right">

                            {
                                row.original.createdCount
                            }

                        </div>

                    ),

                },


                {
                    accessorKey: "updatedCount",

                    header: "Updated",

                    cell: ({ row }) => (

                        <div className="text-right">

                            {
                                row.original.updatedCount
                            }

                        </div>

                    ),

                },


                {
                    accessorKey: "failedCount",

                    header: "Failed",

                    cell: ({ row }) => (

                        <div className="text-right">

                            {
                                row.original.failedCount
                            }

                        </div>

                    ),

                },


                {
                    accessorKey: "ignoredCount",

                    header: "Ignored",

                    cell: ({ row }) => (

                        <div className="text-right">

                            {
                                row.original.ignoredCount
                            }

                        </div>

                    ),

                },

            ],

            []

        );


    function handleRowClick(
        audit: SyncAudit
    ) {

        const auditId =
            audit.id;


        console.log(
            "Opening sync audit:",
            auditId
        );


        router.push(
            `/sync-history/details?id=${auditId}`
        );

    }


    return (

        <Card className="h-full">

            <CardHeader>

                <CardTitle>

                    Synchronization History

                </CardTitle>

            </CardHeader>


            <CardContent
                className="h-[calc(100%-5rem)]"
            >

                <EntityTable

                    columns={columns}

                    data={audits}

                    emptyMessage={
                        "No synchronization history found."
                    }

                    onRowClick={
                        handleRowClick
                    }

                />

            </CardContent>

        </Card>

    );

}