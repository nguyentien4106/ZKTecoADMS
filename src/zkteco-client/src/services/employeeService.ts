
// ==========================================
// src/services/usersService.ts
// ==========================================
import { CreateEmployeeRequest, UpdateEmployeeRequest } from '@/types/employee';
import { apiService } from './api';
import type { Employee } from '@/types/employee';
import { AppResponse } from '@/types';

export const employeeService = {
  getEmployeesByDevices: async (deviceIds?: string[]) => {
    return await apiService.post<Employee[]>('/api/employees/devices', { deviceIds })
  },
  
  getById: (id: string) => apiService.get<Employee>(`/api/employees/${id}`),
  
  getByPin: (pin: string) => apiService.get<Employee>(`/api/employees/pin/${pin}`),
  
  create: async (data: CreateEmployeeRequest) => {
    return await apiService.post<AppResponse<Employee>[]>('/api/employees', data)
  },
  
  update: (data: UpdateEmployeeRequest) => 
    apiService.put<Employee>(`/api/employees/${data.userId}`, data),

  delete: (id: string) => apiService.delete<string>(`/api/employees/${id}`),

};