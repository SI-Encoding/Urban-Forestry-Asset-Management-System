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

export async function apiPost<T>(
    url: string,
    body: unknown
): Promise<T> {

    const response = await fetch(
        `${API_URL}${url}`,
        {
            method: "POST",
            headers: {
                "Content-Type":
                    "application/json",
            },
            body: JSON.stringify(body),
        }
    );

    if (!response.ok) {

        throw new Error(
            `API request failed: ${response.status}`
        );

    }

    return response.json();

}

export async function apiPut<T>(
    url: string,
    body?: unknown
): Promise<T> {

    const response = await fetch(
        `${API_URL}${url}`,
        {
            method: "PUT",
            headers: {
                "Content-Type":
                    "application/json",
            },
            body: body
                ? JSON.stringify(body)
                : undefined,
        }
    );

    if (!response.ok) {

        throw new Error(
            `API request failed: ${response.status}`
        );

    }

    return response.json();

}

export async function apiPatch<T>(
    url: string,
    body: unknown
): Promise<T> {

    const response = await fetch(
        `${API_URL}${url}`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(body),
        });

    if (!response.ok) {
        throw new Error(
            `API request failed: ${response.status}`
        );
    }

    return response.json();
}

export async function apiDelete(
    url: string
): Promise<void> {

    const response = await fetch(
        `${API_URL}${url}`,
        {
            method: "DELETE",
        });

    if (!response.ok) {
        throw new Error(
            `API request failed: ${response.status}`
        );
    }
}