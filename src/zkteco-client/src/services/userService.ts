
// ==========================================
// src/services/usersService.ts
// ==========================================
import { CreateEmployeeRequest, UpdateEmployeeRequest } from '@/types/employee';
import { apiService } from './api';
import type { User, AppResponse } from '@/types';

export const userService = {
  getUsersByDevices: async (deviceIds?: string[]) => {
    return await apiService.post<User[]>('/api/users/devices', { deviceIds })
  },
  
  getById: (id: string) => apiService.get<User>(`/api/users/${id}`),
  
  getByPin: (pin: string) => apiService.get<User>(`/api/users/pin/${pin}`),
  
  create: async (data: CreateEmployeeRequest) => {
    return await apiService.post<AppResponse<User>[]>('/api/users', data)
  },
  
  update: (data: UpdateEmployeeRequest) => 
    apiService.put<User>(`/api/users/${data.userId}`, data),

  delete: (id: string) => apiService.delete<string>(`/api/users/${id}`),
  
  syncToDevice: (userId: string, deviceId: string) => 
    apiService.post(`/api/users/${userId}/sync-to-device/${deviceId}`),
};