import { FieldsViewModel } from "./FieldsViewModel";

export interface TableVieModel {
    columns: FieldsViewModel[]
    data: any[];
    showActions: boolean;
}