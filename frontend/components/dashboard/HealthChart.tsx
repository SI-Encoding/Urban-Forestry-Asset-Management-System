"use client";

import {
    PieChart,
    Pie,
    Cell,
    ResponsiveContainer,
    Tooltip,
    Legend,
} from "recharts";

import type { TreeHealthStatus } from "@/types/tree";

interface HealthChartProps {
    trees: {
        healthStatus: TreeHealthStatus;
    }[];
}

const COLORS = {
    Excellent: "#22c55e",
    Good: "#84cc16",
    Fair: "#eab308",
    Poor: "#f97316",
    Dead: "#ef4444",
};

export function HealthChart({
    trees,
}: HealthChartProps) {

    const statuses: TreeHealthStatus[] = [
        "Excellent",
        "Good",
        "Fair",
        "Poor",
        "Dead",
    ];

    const data = statuses.map(status => ({
        name: status,
        value: trees.filter(
            tree => tree.healthStatus === status
        ).length,
    }));

    return (

        <div className="rounded-lg border bg-white p-6 shadow-sm">

            <h2 className="mb-6 text-lg font-semibold">
                Tree Health Distribution
            </h2>

            <div className="h-80">

                <ResponsiveContainer
                    width="100%"
                    height="100%"
                >

                    <PieChart>

                        <Pie
                            data={data}
                            dataKey="value"
                            nameKey="name"
                            cx="50%"
                            cy="50%"
                            outerRadius={100}
                            label
                        >

                            {data.map(entry => (

                                <Cell
                                    key={entry.name}
                                    fill={
                                        COLORS[
                                            entry.name as TreeHealthStatus
                                        ]
                                    }
                                />

                            ))}

                        </Pie>

                        <Tooltip />

                        <Legend />

                    </PieChart>

                </ResponsiveContainer>

            </div>

        </div>

    );

}