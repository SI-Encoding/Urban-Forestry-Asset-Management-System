"use client";

import { useState } from "react";

import type { Tree } from "@/types/tree";
import type { Inspection } from "@/types/inspection";
import {
    startWorkOrder,
    completeWorkOrder,
    cancelWorkOrder,
} from "@/services/workOrders";
import {
    TreeDetailsTabs,
    type TreeDetailsTab,
} from "./TreeDetailsTabs";
import type { WorkOrder } from "@/types/workOrder";
import { HealthBadge } from "./HealthBadge";
import { EditInspectionForm } from "./EditInspectionForm";
import { CreateInspectionForm } from "./CreateInspectionForm";
import { CreateWorkOrderForm } from "./CreateWorkOrderForm";


interface TreeDetailsPanelProps {
    tree?: Tree;
    inspections: Inspection[];
    workOrders: WorkOrder[];
    onInspectionsChanged: () => void;
    onWorkOrdersChanged: () => void;
}


export function TreeDetailsPanel({
    tree,
    inspections,
    workOrders,
    onInspectionsChanged,
    onWorkOrdersChanged,
}: TreeDetailsPanelProps) {


    const [activeTab, setActiveTab] =
        useState<TreeDetailsTab>("overview");


    const [editingInspectionId, setEditingInspectionId] =
        useState<string | null>(null);

    const [busyWorkOrderId, setBusyWorkOrderId] =
        useState<string | null>(null);
        
    async function handleStartWorkOrder(
        workOrderId: string
    ) {

        await startWorkOrder(workOrderId);

        onWorkOrdersChanged();

    }

    async function handleCompleteWorkOrder(
        workOrderId: string
    ) {

        await completeWorkOrder(workOrderId);

        onWorkOrdersChanged();

    }

    async function handleCancelWorkOrder(
        workOrderId: string
    ) {

        await cancelWorkOrder(workOrderId);

        onWorkOrdersChanged();

    }
    if (!tree) {

        return (

            <div className="rounded-lg border bg-white p-6 shadow-sm">

                <p className="text-gray-500">
                    Select a tree
                </p>

            </div>

        );

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

                        <p>
                            {tree.speciesName}
                        </p>
                    </div>



                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Park
                        </p>

                        <p>
                            {tree.parkName}
                        </p>
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
                        onCreated={onInspectionsChanged}
                    />

                    <hr className="my-6" />


                    <h3 className="mb-4 mt-2 font-semibold">
                        Inspection History
                    </h3>



                    {
                        sortedInspections.length === 0

                        ? (

                            <p className="text-sm text-gray-500">
                                No inspections found.
                            </p>

                        )

                        : (

                            <div className="space-y-4">

                                {
                                    sortedInspections.map(
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
                                                        {
                                                            inspection.inspectionDate
                                                        }
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
                                                        {
                                                            inspection.notes
                                                        }
                                                    </p>

                                                </div>



                                                <div className="mt-3">

                                                    <p className="text-xs uppercase text-gray-500">
                                                        Recommendation
                                                    </p>

                                                    <p>
                                                        {
                                                            inspection.recommendation
                                                        }
                                                    </p>

                                                </div>



                                                <div className="mt-3">

                                                    <p className="text-xs uppercase text-gray-500">
                                                        Next Inspection
                                                    </p>

                                                    <p>
                                                        {
                                                            inspection.nextInspectionDate
                                                            ??
                                                            "Not scheduled"
                                                        }
                                                    </p>

                                                </div>



                                                <div className="mt-4">

                                                    {
                                                        editingInspectionId === inspection.id

                                                        ? (

                                                            <EditInspectionForm
                                                                inspection={
                                                                    inspection
                                                                }

                                                                onSaved={() => {

                                                                    setEditingInspectionId(
                                                                        null
                                                                    );

                                                                    onInspectionsChanged();

                                                                }}

                                                                onCancel={() => {

                                                                    setEditingInspectionId(
                                                                        null
                                                                    );

                                                                }}
                                                            />

                                                        )

                                                        : (

                                                            <button
                                                                onClick={() =>
                                                                    setEditingInspectionId(
                                                                        inspection.id
                                                                    )
                                                                }

                                                                className="
                                                                    rounded
                                                                    border
                                                                    px-3
                                                                    py-1
                                                                    text-sm
                                                                "
                                                            >
                                                                Edit
                                                            </button>

                                                        )
                                                    }

                                                </div>


                                            </div>

                                        )
                                    )
                                }

                            </div>

                        )
                    }


                </>

            )}
            {activeTab === "workorders" && (

            <>
                <CreateWorkOrderForm
                    treeId={tree.id}
                    onCreated={onWorkOrdersChanged}
                />

                <hr className="my-6" />
                <h3 className="mb-4 mt-2 font-semibold">
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
                                        className={`
                                            rounded-full
                                            px-3
                                            py-1
                                            text-xs
                                            font-medium

                                            ${
                                                workOrder.status === "Open"
                                                    ? "bg-yellow-100 text-yellow-800"
                                                : workOrder.status === "InProgress"
                                                    ? "bg-blue-100 text-blue-800"
                                                : workOrder.status === "Completed"
                                                    ? "bg-green-100 text-green-800"
                                                : "bg-red-100 text-red-800"
                                            }
                                        `}
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
                                            <button
                                                onClick={() =>
                                                    handleStartWorkOrder(workOrder.id)
                                                }
                                                className="rounded bg-blue-600 px-3 py-1 text-sm text-white"
                                            >
                                                Start
                                            </button>

                                            <button
                                                onClick={() =>
                                                    handleCancelWorkOrder(workOrder.id)
                                                }
                                                className="rounded bg-red-600 px-3 py-1 text-sm text-white"
                                            >
                                                Cancel
                                            </button>
                                        </>
                                    )}

                                    {workOrder.status === "InProgress" && (
                                        <>
                                            <button
                                                onClick={() =>
                                                    handleCompleteWorkOrder(workOrder.id)
                                                }
                                                className="rounded bg-green-600 px-3 py-1 text-sm text-white"
                                            >
                                                Complete
                                            </button>

                                            <button
                                                onClick={() =>
                                                    handleCancelWorkOrder(workOrder.id)
                                                }
                                                className="rounded bg-red-600 px-3 py-1 text-sm text-white"
                                            >
                                                Cancel
                                            </button>
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