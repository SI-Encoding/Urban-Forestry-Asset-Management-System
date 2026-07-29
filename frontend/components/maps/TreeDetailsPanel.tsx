"use client";

import { useState } from "react";

import type { Tree } from "@/types/tree";
import type { Inspection } from "@/types/inspection";
import type { WorkOrder } from "@/types/workOrder";

import {
    startWorkOrder,
    completeWorkOrder,
    cancelWorkOrder,
} from "@/services/workOrders";

import {
    TreeDetailsTabs,
    type TreeDetailsTab,
} from "./TreeDetailsTabs";

import { HealthBadge } from "../ui/HealthBadge";
import { Button } from "../ui/button";

import { EditInspectionForm } from "./EditInspectionForm";
import { CreateInspectionForm } from "./CreateInspectionForm";
import { CreateWorkOrderForm } from "./CreateWorkOrderForm";

interface TreeDetailsPanelProps {
    tree?: Tree;
    inspections: Inspection[];
    workOrders: WorkOrder[];
}

export function TreeDetailsPanel({
    tree,
    inspections,
    workOrders,
}: TreeDetailsPanelProps) {

    const [activeTab, setActiveTab] =
        useState<TreeDetailsTab>("overview");

    const [
        editingInspectionId,
        setEditingInspectionId,
    ] = useState<string | null>(null);

    if (!tree) {

        return (

            <div
                className="
                    w-[420px]
                    shrink-0
                    rounded-lg
                    border
                    bg-white
                    p-6
                    shadow-sm
                "
            >

                <p className="text-gray-500">
                    Select a tree
                </p>

            </div>

        );

    }

    async function handleStartWorkOrder(
        workOrderId: string
    ) {

        await startWorkOrder(workOrderId);

        window.location.reload();

    }

    async function handleCompleteWorkOrder(
        workOrderId: string
    ) {

        await completeWorkOrder(workOrderId);

        window.location.reload();

    }

    async function handleCancelWorkOrder(
        workOrderId: string
    ) {

        await cancelWorkOrder(workOrderId);

        window.location.reload();

    }

    const sortedInspections =
        [...inspections].sort(
            (a, b) =>
                new Date(b.inspectionDate).getTime()
                -
                new Date(a.inspectionDate).getTime()
        );

    return (

        <div
            className="
                w-[420px]
                shrink-0
                h-full
                overflow-y-auto
                rounded-lg
                border
                bg-white
                p-6
                shadow-sm
            "
        >

            <h2 className="mb-1 text-lg font-semibold">
                {tree.assetTag}
            </h2>

            <p className="mb-6 text-sm text-gray-500">
                {tree.speciesName}
            </p>

            <TreeDetailsTabs
                active={activeTab}
                onChange={setActiveTab}
            />

            {activeTab === "overview" && (

                <div className="space-y-4">

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Asset Tag
                        </p>

                        <p className="font-medium">
                            {tree.assetTag}
                        </p>
                    </div>

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Species
                        </p>

                        <p>{tree.speciesName}</p>
                    </div>

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Park
                        </p>

                        <p>{tree.parkName}</p>
                    </div>

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Current Health
                        </p>

                        <HealthBadge
                            status={tree.healthStatus}
                        />
                    </div>

                </div>

            )}

            {activeTab === "inspections" && (

                <>

                    <CreateInspectionForm
                        treeId={tree.id}
                        onCreated={() => window.location.reload()}
                    />

                    <hr className="my-6" />

                    <h3 className="mb-4 font-semibold">
                        Inspection History
                    </h3>

                    {sortedInspections.length === 0 ? (

                        <p className="text-sm text-gray-500">
                            No inspections found.
                        </p>

                    ) : (

                        <div className="space-y-4">

                            {sortedInspections.map(
                                (inspection) => (

                                    <div
                                        key={inspection.id}
                                        className="
                                            rounded-md
                                            border
                                            p-4
                                        "
                                    >

                                        <div
                                            className="
                                                flex
                                                items-center
                                                justify-between
                                            "
                                        >

                                            <span className="font-medium">
                                                {inspection.inspectionDate}
                                            </span>

                                            <HealthBadge
                                                status={
                                                    inspection.observedHealth
                                                }
                                            />

                                        </div>

                                        <div className="mt-3">
                                            <p className="text-xs uppercase text-gray-500">
                                                Notes
                                            </p>

                                            <p>
                                                {inspection.notes}
                                            </p>
                                        </div>

                                        <div className="mt-3">
                                            <p className="text-xs uppercase text-gray-500">
                                                Recommendation
                                            </p>

                                            <p>
                                                {inspection.recommendation}
                                            </p>
                                        </div>

                                        <div className="mt-3">
                                            <p className="text-xs uppercase text-gray-500">
                                                Next Inspection
                                            </p>

                                            <p>
                                                {inspection.nextInspectionDate ??
                                                    "Not scheduled"}
                                            </p>
                                        </div>

                                        <div className="mt-4">

                                            {editingInspectionId === inspection.id ? (

                                                <EditInspectionForm
                                                    inspection={inspection}
                                                    onSaved={() => {

                                                        setEditingInspectionId(null);

                                                        window.location.reload();

                                                    }}
                                                    onCancel={() =>
                                                        setEditingInspectionId(null)
                                                    }
                                                />

                                            ) : (

                                                <Button
                                                    variant="outline"
                                                    size="sm"
                                                    onClick={() =>
                                                        setEditingInspectionId(
                                                            inspection.id
                                                        )
                                                    }
                                                >
                                                    Edit
                                                </Button>

                                            )}

                                        </div>

                                    </div>

                                )
                            )}

                        </div>

                    )}

                </>

            )}

            {activeTab === "workorders" && (

                <>

                    <CreateWorkOrderForm
                        treeId={tree.id}
                        onCreated={() => window.location.reload()}
                    />

                    <hr className="my-6" />

                    <h3 className="mb-4 font-semibold">
                        Work Orders
                    </h3>

                    {workOrders.length === 0 ? (

                        <p className="text-sm text-gray-500">
                            No work orders found.
                        </p>

                    ) : (

                        <div className="space-y-4">

                            {workOrders.map((workOrder) => (

                                <div
                                    key={workOrder.id}
                                    className="rounded-md border p-4"
                                >

                                    <div className="flex justify-between">

                                        <span className="font-medium">
                                            {workOrder.createdDate}
                                        </span>

                                        <span
                                            className="
                                                rounded-full
                                                border
                                                px-3
                                                py-1
                                                text-xs
                                                font-medium
                                            "
                                        >
                                            {workOrder.status}
                                        </span>

                                    </div>

                                    <div className="mt-3">

                                        <p className="text-xs uppercase text-gray-500">
                                            Description
                                        </p>

                                        <p>
                                            {workOrder.description}
                                        </p>

                                    </div>

                                    <div className="mt-3">

                                        <p className="text-xs uppercase text-gray-500">
                                            Due Date
                                        </p>

                                        <p>
                                            {workOrder.dueDate}
                                        </p>

                                    </div>

                                    <div className="mt-3">

                                        <p className="text-xs uppercase text-gray-500">
                                            Completed
                                        </p>

                                        <p>
                                            {workOrder.completedDate ??
                                                "Not completed"}
                                        </p>

                                    </div>

                                    <div className="mt-4 flex gap-2">

                                        {workOrder.status === "Open" && (

                                            <>

                                                <Button
                                                    size="sm"
                                                    onClick={() =>
                                                        handleStartWorkOrder(
                                                            workOrder.id
                                                        )
                                                    }
                                                >
                                                    Start
                                                </Button>

                                                <Button
                                                    variant="destructive"
                                                    size="sm"
                                                    onClick={() =>
                                                        handleCancelWorkOrder(
                                                            workOrder.id
                                                        )
                                                    }
                                                >
                                                    Cancel
                                                </Button>

                                            </>

                                        )}

                                        {workOrder.status === "InProgress" && (

                                            <>

                                                <Button
                                                    size="sm"
                                                    onClick={() =>
                                                        handleCompleteWorkOrder(
                                                            workOrder.id
                                                        )
                                                    }
                                                >
                                                    Complete
                                                </Button>

                                                <Button
                                                    variant="destructive"
                                                    size="sm"
                                                    onClick={() =>
                                                        handleCancelWorkOrder(
                                                            workOrder.id
                                                        )
                                                    }
                                                >
                                                    Cancel
                                                </Button>

                                            </>

                                        )}

                                    </div>

                                </div>

                            ))}

                        </div>

                    )}

                </>

            )}

        </div>

    );

}