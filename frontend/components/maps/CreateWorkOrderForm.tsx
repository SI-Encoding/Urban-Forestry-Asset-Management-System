"use client";

import { useState } from "react";

import { createWorkOrder } from "@/services/workOrders";

interface CreateWorkOrderFormProps {
    treeId: string;
    onCreated: () => void;
}

export function CreateWorkOrderForm({
    treeId,
    onCreated,
}: CreateWorkOrderFormProps) {
    const [success, setSuccess] =
        useState("");
    const [description, setDescription] =
        useState("");

    const [dueDate, setDueDate] =
        useState("");

    const [error, setError] = useState("");

    async function submit() {

        setError("");

        if (!description.trim()) {

            setError("Description is required.");

            return;
        }

        try {

            await createWorkOrder(
                treeId,
                {
                    description,
                    dueDate: dueDate || undefined,
                }
            );

            setDescription("");
            setDueDate("");
            setSuccess(
                "Work order created successfully."
            );
            onCreated();

        } catch {

            setError(
                "Unable to create work order."
            );

        }

    }

    return (

        <div className="space-y-4">
            <h3 className="font-semibold">
                New Work Order
            </h3>
            {success && (

                <p className="
                    rounded
                    bg-green-50
                    p-2
                    text-sm
                    text-green-700
                ">
                    {success}
                </p>

            )}
            {error && (
                <div
                    className="
                    rounded
                    bg-red-50
                    p-2
                    text-sm
                  text-red-600
                    "
                >
                    {error}
                </div>
            )}    
            <textarea
                className="w-full rounded border p-2"
                placeholder="Description"
                value={description}
                onChange={(e) =>
                    setDescription(
                        e.target.value
                    )
                }
            />

            <input
                type="date"
                className="w-full rounded border p-2"
                value={dueDate}
                onChange={(e) =>
                    setDueDate(
                        e.target.value
                    )
                }
            />
            
            <button
                onClick={submit}
                className="rounded bg-blue-600 px-4 py-2 text-white"
            >
                Create Work Order
            </button>

        </div>

    );

}