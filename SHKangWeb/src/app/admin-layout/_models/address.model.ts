import { BaseModel } from '../../shared/crud-table';

export class Address {
  status?: number;
  id: number
  name?: string
  full_name?: string
  company_name?: string
  state?: string
  city?: string
  country?: string
  zipcode?: string
  email?: string
  phone?: string;
  address1: string;
}