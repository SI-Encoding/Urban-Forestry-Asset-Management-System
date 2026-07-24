import type { Tree } from "@/types/tree";

interface TreeDetailsPanelProps {
    tree?: Tree;
}

export function TreeDetailsPanel({
    tree,
}: TreeDetailsPanelProps) {

    if (!tree) {
        return (
            <div className="p-6">
                Select a tree
            </div>
        );
    }

    return (
        <div className="p-6 space-y-3">
            <h2 className="text-xl font-bold">
                Tree Details
            </h2>

            <div>
                <strong>Asset:</strong>{" "}
                {tree.assetTag}
            </div>

            <div>
                <strong>Species:</strong>{" "}
                {tree.speciesName}
            </div>

            <div>
                <strong>Park:</strong>{" "}
                {tree.parkName}
            </div>

            <div>
                <strong>Health:</strong>{" "}
                {tree.healthStatus}
            </div>
        </div>
    );
}