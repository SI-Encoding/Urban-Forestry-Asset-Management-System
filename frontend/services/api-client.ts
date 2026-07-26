const API_URL =
  process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5079";

export async function apiFetch<T>(
    url: string,
    options?: RequestInit
): Promise<T> {

    const response = await fetch(
        `${API_URL}${url}`,
        {
            cache: "no-store",
            headers: {
                "Content-Type": "application/json",
            },
            ...options,
        }
    );

    if (!response.ok) {
        throw new Error(
            `API request failed: ${response.status}`
        );
    }

    return response.json();
}