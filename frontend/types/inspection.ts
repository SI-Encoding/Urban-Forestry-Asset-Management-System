export interface Inspection {
  id: string;
  treeId: string;
  assetTag: string;
  speciesName: string;
  parkName: string;
  inspectionDate: string;
  observedHealth: 
    | "Excellent"
    | "Good"
    | "Fair"
    | "Poor"
    | "Dead";
  notes: string;
  recommendation: string;
  nextInspectionDate: string | null;
}