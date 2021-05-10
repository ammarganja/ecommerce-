import { Color } from "./color.model";
import { UserModel } from "./user.model";

export class Orders {
    id: number;
    date: string;
    user?: UserModel;
    pono: string;
    size_ratio: string;
    ratio: string;
    invoice: any[];
    color: Color;
    amount: number;
    unitprice: number;
    qty: number;
    paymentStatus: number;
    orderstatus: number;
    idproducts: string;
    pendinginvoice: number;
    status?:number;
}