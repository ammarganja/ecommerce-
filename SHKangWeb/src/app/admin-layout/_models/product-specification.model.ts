import { BaseModel } from "src/app/shared/crud-table";

export interface ProductSpecification extends BaseModel {
    id: number;
    carId: number;
    specId: number;
    specName: string;
    value: string;
  }