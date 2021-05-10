import { BaseModel } from '../../shared/crud-table';

export class Login {
  id: number;
  email: string;
  password: string;
  token: string;
  status: number;
}
