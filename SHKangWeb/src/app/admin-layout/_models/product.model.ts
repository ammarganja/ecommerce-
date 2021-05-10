import { BaseModel } from '../../shared/crud-table';

export interface Product extends BaseModel {
  carId: number;
  status?: number;
  productId: string;
  productDescription: string;
  productName :string;
  price:string;
  isDelete:string;
  categoryType :string;
  color:string;
  image:string;
  
}
