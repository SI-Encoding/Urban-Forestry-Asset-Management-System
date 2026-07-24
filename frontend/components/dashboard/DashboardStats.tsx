import { StatCard } from "./StatCard";

interface DashboardStatsProps {
    trees: number;
    parks: number;
    species: number;
}

export function DashboardStats({
    trees,
    parks,
    species,
}: DashboardStatsProps) {
    return (
        <div className="grid gap-6 md:grid-cols-3">
            <StatCard
                title="Trees"
                value={trees}
            />

            <StatCard
                title="Parks"
                value={parks}
            />

            <StatCard
                title="Species"
                value={species}
            />
        </div>
    );
}