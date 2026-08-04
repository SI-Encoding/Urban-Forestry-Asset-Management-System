import type {
    SpatialSyncResult,
} from "@/types/arcgisSync";

interface Props {
    result: SpatialSyncResult;
}

export function ArcGisSyncSummary({
    result,
}: Props) {

    return (

        <div className="grid grid-cols-4 gap-4">

            <Card
                title="Created"
                value={result.created}
            />

            <Card
                title="Updated"
                value={result.updated}
            />

            <Card
                title="Deleted"
                value={result.deleted}
            />

            <Card
                title="Unchanged"
                value={result.unchanged}
            />

        </div>

    );
}


function Card({
    title,
    value,
}: {
    title: string;
    value: number;
}) {

    return (

        <div className="rounded-lg border bg-white p-6">

            <p className="text-sm text-muted-foreground">
                {title}
            </p>

            <p className="mt-2 text-3xl font-bold">
                {value}
            </p>

        </div>

    );
}