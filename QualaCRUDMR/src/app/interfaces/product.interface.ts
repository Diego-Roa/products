export interface Product {
    id: number;
    codeProduct: number;
    name: string;
    description: string;
    reference: string;
    unitPrice: number;
    status: boolean;
    unitMeasurement: string;
    createdAt: Date;
}