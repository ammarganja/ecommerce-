import { BaseModel } from '../../shared/crud-table';

export interface Category extends BaseModel {
  status: number;
  productCategoryTypeId: number;
  name: string;
  totalProduct: number;
}