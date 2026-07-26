const API_URL =
  process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5079";

export async function apiFetch<T>(url: string): Promise<T> {

    const fullUrl = `${API_URL}${url}`;

    console.log("Fetching:", fullUrl);

    const response = await fetch(fullUrl, {
        cache: "no-store",
    });

    if (!response.ok) {
        throw new Error(`API request failed: ${response.status}`);
    }

    return response.json();
}