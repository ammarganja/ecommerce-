import { BaseModel } from '../../shared/crud-table';

export interface Color extends BaseModel {
  status?: number;
  id: number;
  name: string;
}