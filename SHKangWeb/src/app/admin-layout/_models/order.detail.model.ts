import { BaseModel } from '../../shared/crud-table';
import { Address } from './address.model';
import { OrderProducts } from './order.product.model';
import { UserModel } from './user.model';

export interface OrderDetail extends BaseModel {
    id: number;
    orderno: string;
    createdDate?: Date;
    address: Address;
    user: UserModel;
    products: OrderProducts[];
    totalAmount?: 0,
    vat: number;
    shippingCharge: number;
    TrackingNo: string;
}
