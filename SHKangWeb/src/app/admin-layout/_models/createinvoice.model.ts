import { BaseModel } from '../../shared/crud-table';

export interface createinvoice extends BaseModel {
  id: number;
  fullname: string;
  // lastName: string;
  email: string;
  userName: string;
  gender: string;
  status: number; // Active = 1 | Suspended = 2 | Pending = 3
  dateOfBbirth: string;
  dob: Date;
  ipAddress: string;
  type: number; // 1 = Business | 2 = Individual
  company: string;
  phone_number: string;
  address: any;
  line_code: string;
  country:string;
  state:string;
  city:string;
  zipcode:string;
  street1:string;
  street2: string;
  _userId:number;
  _createdDate:string;
  _updatedDate:string;
}