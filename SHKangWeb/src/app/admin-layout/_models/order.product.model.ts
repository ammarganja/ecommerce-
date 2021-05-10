import { Orders } from "./orders.model";
import { Product } from "./product.model";

export class OrderProducts {
    id: number;
    name: string;
    image: string;
    style: string;
    orderSmallRatio?: {
        S: '',
        Total: '',
        hasError: boolean
    };
    orderMideumRatio?: {
        M: '',
        Total: '',
        hasError: boolean
    };
    orderLargeRatio?: {
        L: '',
        Total: '',
        hasError: boolean
    };
    ratio?: string;
    requiredSizeRatio: string;
    shippedSizeRatio: string;
    remainSizeRatio: string;
    qty: number;
    unitPrice: number;
    isSelected?: boolean;
    totalAmount: number;
}