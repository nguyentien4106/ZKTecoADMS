import { Account } from "./account";

export interface CurrentSalaryProfile {
  id: string;
  salaryProfileId: string;
  profileName: string;
  rate: number;
  currency: string;
  rateTypeName: string;
  effectiveDate: string;
  isActive: boolean;
}

export interface CreateEmployeeRequest {
    pin: string;
    name: string;
    cardNumber?: string;
    password?: string;
    privilege?: number;
    department?: string;
    deviceIds?: string[];
}

export interface UpdateEmployeeRequest {
    userId: string
    pin: string;
    name: string;
    cardNumber?: string;
    password?: string;
    privilege?: number;
    department?: string;
    deviceId: string;
}

export interface Employee {
  id: string;
  pin: string;
  name: string;
  password: string;
  cardNumber: string;
  department: string;
  isActive: boolean;
  privilege: 0 | 1 | 2 | 14;
  createdAt: string;
  updatedAt: string;
  deviceId: string;
  deviceName?: string;
  applicationUser?: Account | null;
  currentSalaryProfile?: CurrentSalaryProfile | null;
}
