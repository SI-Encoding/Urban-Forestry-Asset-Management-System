import {
    apiFetch,
} from "./api-client";


export interface ArcGisAuthStatus {

    authenticated: boolean;

}



export async function getArcGisAuthStatus()
    : Promise<ArcGisAuthStatus> {

    return apiFetch<ArcGisAuthStatus>(
        "/api/arcgis/auth/status"
    );

}



export function getArcGisLoginUrl(): string {

    const apiUrl =
        process.env.NEXT_PUBLIC_API_URL ??
        "http://localhost:5079";


    return (
        `${apiUrl}`
        +
        "/api/arcgis/auth/login"
    );

}