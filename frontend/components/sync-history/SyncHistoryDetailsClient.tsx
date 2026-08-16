"use client";

import {
    useEffect,
    useState,
} from "react";

import Link from "next/link";

import {
    useSearchParams,
} from "next/navigation";

import {
    getSyncAudits,
} from "@/services/sync-audit";

import type {
    SyncAudit,
} from "@/services/sync-audit";

import {
    AppLayout,
} from "@/components/layout/AppLayout";

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";


export default function SyncHistoryDetailsClient() {

    const searchParams =
        useSearchParams();


    const id =
        searchParams.get("id");


    const auditId =
        id
            ? Number(id)
            : null;


    const isValidAuditId =
        auditId !== null &&
        Number.isInteger(auditId) &&
        auditId > 0;


    const [audit, setAudit] =
        useState<SyncAudit | null>(null);


    const [loading, setLoading] =
        useState(isValidAuditId);


    useEffect(() => {

        if (
            !isValidAuditId ||
            auditId === null
        ) {
            return;
        }


        let cancelled = false;


        getSyncAudits()

            .then(audits => {

                if (cancelled) {
                    return;
                }


                const found =
                    audits.find(
                        item =>
                            item.id === auditId
                    );


                setAudit(
                    found ?? null
                );

            })

            .catch(() => {

                if (!cancelled) {
                    setAudit(null);
                }

            })

            .finally(() => {

                if (!cancelled) {
                    setLoading(false);
                }

            });


        return () => {

            cancelled = true;

        };


    }, [
        auditId,
        isValidAuditId,
    ]);


    /*
     * No ID was supplied.
     */

    if (!id) {

        return (

            <AppLayout>

                <div className="p-6">

                    <h1 className="text-2xl font-bold">

                        Synchronization Details

                    </h1>


                    <p className="mt-2 text-muted-foreground">

                        No synchronization audit ID was provided.

                    </p>

                </div>

            </AppLayout>

        );

    }


    /*
     * ID was supplied but is not valid.
     */

    if (!isValidAuditId) {

        return (

            <AppLayout>

                <div className="p-6">

                    <h1 className="text-2xl font-bold">

                        Synchronization Details

                    </h1>


                    <p className="mt-2 text-muted-foreground">

                        Invalid synchronization audit ID.

                    </p>

                </div>

            </AppLayout>

        );

    }


    /*
     * API request is still running.
     */

    if (loading) {

        return (

            <AppLayout>

                <div className="p-6">

                    <h1 className="text-2xl font-bold">

                        Synchronization Details

                    </h1>


                    <p className="mt-2 text-muted-foreground">

                        Loading synchronization details...

                    </p>

                </div>

            </AppLayout>

        );

    }


    /*
     * Audit was not found.
     */

    if (!audit) {

        return (

            <AppLayout>

                <div className="p-6">

                    <h1 className="text-2xl font-bold">

                        Synchronization Details

                    </h1>


                    <p className="mt-2 text-muted-foreground">

                        Synchronization audit #{auditId} was not found.

                    </p>

                </div>

            </AppLayout>

        );

    }


    /*
     * Audit successfully loaded.
     */

    return (

        <AppLayout>

            <div
                className="
                    flex
                    flex-col
                    h-full
                    min-h-0
                    space-y-6
                "
            >

                <div className="shrink-0">


                    <Link
                        href="/sync-history"
                        className="
                            inline-flex
                            items-center
                            rounded-md
                            border
                            px-3
                            py-2
                            text-sm
                            font-medium
                            hover:bg-muted
                            transition
                        "
                    >
                        ← Back to Sync History
                    </Link>


                    <div className="mt-4">

                        <h1 className="text-3xl font-bold">

                            Synchronization Details

                        </h1>


                        <p className="text-muted-foreground">

                            Details of ArcGIS synchronization operation #

                            {audit.id}.

                        </p>

                    </div>

                </div>


                <div
                    className="
                        grid
                        grid-cols-2
                        md:grid-cols-5
                        gap-4
                        shrink-0
                    "
                >

                    <Card>

                        <CardHeader>

                            <CardTitle className="text-sm">

                                Status

                            </CardTitle>

                        </CardHeader>


                        <CardContent>

                            <div className="text-lg font-semibold">

                                {audit.status}

                            </div>

                        </CardContent>

                    </Card>


                    <Card>

                        <CardHeader>

                            <CardTitle className="text-sm">

                                Created

                            </CardTitle>

                        </CardHeader>


                        <CardContent>

                            <div className="text-lg font-semibold">

                                {audit.createdCount}

                            </div>

                        </CardContent>

                    </Card>


                    <Card>

                        <CardHeader>

                            <CardTitle className="text-sm">

                                Updated

                            </CardTitle>

                        </CardHeader>


                        <CardContent>

                            <div className="text-lg font-semibold">

                                {audit.updatedCount}

                            </div>

                        </CardContent>

                    </Card>


                    <Card>

                        <CardHeader>

                            <CardTitle className="text-sm">

                                Failed

                            </CardTitle>

                        </CardHeader>


                        <CardContent>

                            <div className="text-lg font-semibold">

                                {audit.failedCount}

                            </div>

                        </CardContent>

                    </Card>


                    <Card>

                        <CardHeader>

                            <CardTitle className="text-sm">

                                Ignored

                            </CardTitle>

                        </CardHeader>


                        <CardContent>

                            <div className="text-lg font-semibold">

                                {audit.ignoredCount}

                            </div>

                        </CardContent>

                    </Card>

                </div>


                <Card className="flex-1 min-h-0">

                    <CardHeader>

                        <CardTitle>

                            Synchronization Operations

                        </CardTitle>

                    </CardHeader>


                    <CardContent className="h-[calc(100%-5rem)]">


                        {audit.entries.length === 0 ? (

                            <div
                                className="
                                    py-8
                                    text-center
                                    text-muted-foreground
                                "
                            >

                                No synchronization operations were recorded.

                            </div>

                        ) : (

                            <div
                                className="
                                    h-full
                                    overflow-auto
                                    rounded-md
                                    border
                                "
                            >

                                <Table>

                                    <TableHeader>

                                        <TableRow>

                                            <TableHead>

                                                Asset Tag

                                            </TableHead>


                                            <TableHead>

                                                Action

                                            </TableHead>


                                            <TableHead>

                                                Reason

                                            </TableHead>


                                            <TableHead>

                                                Date

                                            </TableHead>

                                        </TableRow>

                                    </TableHeader>


                                    <TableBody>

                                        {audit.entries.map(

                                            entry => (

                                                <TableRow
                                                    key={entry.id}
                                                >

                                                    <TableCell>

                                                        {entry.assetTag}

                                                    </TableCell>


                                                    <TableCell>

                                                        {entry.action}

                                                    </TableCell>


                                                    <TableCell>

                                                        {entry.reason}

                                                    </TableCell>


                                                    <TableCell>

                                                        {new Date(
                                                            entry.createdAt
                                                        ).toLocaleString(
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
                                                        )}

                                                    </TableCell>

                                                </TableRow>

                                            )

                                        )}

                                    </TableBody>

                                </Table>

                            </div>

                        )}

                    </CardContent>

                </Card>

            </div>

        </AppLayout>

    );

}