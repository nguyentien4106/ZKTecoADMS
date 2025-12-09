import { apiService } from './api';

export enum SalaryRateType {
  Hourly = 1,
  Daily = 2,
  Monthly = 3,
  Yearly = 4
}

export interface SalaryProfile {
  id: string;
  name: string;
  description?: string;
  rateType: SalaryRateType;
  rateTypeName: string;
  rate: number;
  currency: string;
  overtimeMultiplier?: number;
  holidayMultiplier?: number;
  nightShiftMultiplier?: number;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface EmployeeSalaryProfile {
  id: string;
  employeeId: string;
  employeeName: string;
  salaryProfileId: string;
  salaryProfile: SalaryProfile;
  effectiveDate: string;
  endDate?: string;
  isActive: boolean;
  notes?: string;
  createdAt: string;
}

export interface CreateSalaryProfileRequest {
  name: string;
  description?: string;
  rateType: SalaryRateType;
  rate: number;
  currency: string;
  overtimeMultiplier?: number;
  holidayMultiplier?: number;
  nightShiftMultiplier?: number;
}

export interface UpdateSalaryProfileRequest extends CreateSalaryProfileRequest {
  isActive: boolean;
}

export interface AssignSalaryProfileRequest {
  employeeId: string;
  salaryProfileId: string;
  effectiveDate: string;
  notes?: string;
}

export interface AppResponse<T> {
  data: T;
  isSuccess: boolean;
  message: string;
}

const salaryProfileService = {
  // Get all salary profiles
  getAllProfiles: async (activeOnly?: boolean): Promise<SalaryProfile[]> => {
    const params = activeOnly !== undefined ? `?activeOnly=${activeOnly}` : '';
    return await apiService.get<SalaryProfile[]>(`/api/salaryprofiles${params}`);
  },

  // Get salary profile by ID
  getProfileById: async (id: string): Promise<SalaryProfile> => {
    return await apiService.get<SalaryProfile>(`/api/salaryprofiles/${id}`);
  },

  // Create new salary profile
  createProfile: async (request: CreateSalaryProfileRequest): Promise<SalaryProfile> => {
    return await apiService.post<SalaryProfile>('/api/salaryprofiles', request);
  },

  // Update salary profile
  updateProfile: async (id: string, request: UpdateSalaryProfileRequest): Promise<SalaryProfile> => {
    return await apiService.put<SalaryProfile>(`/api/salaryprofiles/${id}`, request);
  },

  // Delete salary profile
  deleteProfile: async (id: string): Promise<boolean> => {
    return await apiService.delete<boolean>(`/api/salaryprofiles/${id}`);
  },

  // Assign salary profile to employee
  assignProfile: async (request: AssignSalaryProfileRequest): Promise<EmployeeSalaryProfile> => {
    return await apiService.post<EmployeeSalaryProfile>('/api/salaryprofiles/assign', request);
  },

  // Get employee's active salary profile
  getEmployeeSalaryProfile: async (employeeId: string): Promise<EmployeeSalaryProfile> => {
    return await apiService.get<EmployeeSalaryProfile>(`/api/salaryprofiles/employee/${employeeId}`);
  }
};

export default salaryProfileService;
