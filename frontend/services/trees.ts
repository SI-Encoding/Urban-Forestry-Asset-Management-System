import { apiFetch } from "./api-client";
import type { Tree } from "@/types/tree";

export function getTrees() {
    return apiFetch<Tree[]>("/trees");
}