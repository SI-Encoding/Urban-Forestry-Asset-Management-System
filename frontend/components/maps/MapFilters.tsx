"use client";

import type { TreeHealthStatus } from "@/types/tree";

interface MapFiltersProps {
    selected: TreeHealthStatus[];
    onChange: (health: TreeHealthStatus) => void;
}

const statuses: TreeHealthStatus[] = [
    "Excellent",
    "Good",
    "Fair",
    "Poor",
    "Dead",
];

export function MapFilters({
    selected,
    onChange,
}: MapFiltersProps) {
    return (
        <div className="rounded-lg border bg-white p-4 shadow-sm">

            <h2 className="mb-4 font-semibold">
                Tree Health
            </h2>

            <div className="space-y-2">

                {statuses.map((status) => (

                    <label
                        key={status}
                        className="flex items-center gap-2"
                    >
                        <input
                            type="checkbox"
                            checked={selected.includes(status)}
                            onChange={() => onChange(status)}
                        />

                        {status}

                    </label>

                ))}

            </div>

        </div>
    );
}