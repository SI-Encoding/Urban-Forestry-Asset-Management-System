import type { TreeHealthStatus } from "@/types/tree";

interface HealthBadgeProps {
    status: TreeHealthStatus;
}

const colors: Record<TreeHealthStatus, string> = {
    Excellent: "bg-blue-100 text-blue-700",
    Good: "bg-green-100 text-green-700",
    Fair: "bg-yellow-100 text-yellow-700",
    Poor: "bg-orange-100 text-orange-700",
    Dead: "bg-red-100 text-red-700",
};

const dots: Record<TreeHealthStatus, string> = {
    Excellent: "bg-blue-500",
    Good: "bg-green-500",
    Fair: "bg-yellow-500",
    Poor: "bg-orange-500",
    Dead: "bg-red-500",
};

export function HealthBadge({
    status,
}: HealthBadgeProps) {

    return (
        <span
            className={`
                inline-flex items-center gap-2
                rounded-full px-3 py-1 text-sm
                ${colors[status]}
            `}
        >
            <span
                className={`
                    h-2 w-2 rounded-full
                    ${dots[status]}
                `}
            />

            {status}

        </span>
    );
}