import { apiFetch } from "./api-client";
import type { Park } from "@/types/park";

export function getParks() {
    return apiFetch<Park[]>("/parks");
}