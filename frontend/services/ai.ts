const API_URL =
    process.env.NEXT_PUBLIC_API_URL ??
    "http://localhost:5079";

export interface TreeAiSummary {
    assetTag: string;
    summary: string;
}

export async function generateTreeSummary(
    treeId: string
): Promise<TreeAiSummary> {

    const response = await fetch(
        `${API_URL}/api/ai/tree-summary/${treeId}`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Accept: "application/json",
            },
        }
    );

    if (!response.ok) {

        let message =
            "Unable to generate AI assessment.";

        try {

            const error =
                await response.json();

            if (error?.message) {
                message = error.message;
            }

        } catch {
            // Ignore JSON parsing errors.
        }

        throw new Error(message);
    }

    return response.json();
}