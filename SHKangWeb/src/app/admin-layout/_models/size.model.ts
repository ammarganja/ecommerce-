import { BaseModel } from '../../shared/crud-table';

export interface Size extends BaseModel {
  status: number;
  id:string
  name:string
}
