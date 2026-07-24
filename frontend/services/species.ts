import { apiFetch } from "./api-client";
import type { Species } from "@/types/species";

export function getSpecies() {
    return apiFetch<Species[]>("/species");
}