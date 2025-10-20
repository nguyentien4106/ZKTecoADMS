
// ==========================================
// src/services/usersService.ts
// ==========================================
import { apiService } from './api';
import type { User, CreateUserRequest, UserDeviceMapping, AppResponse } from '@/types';

export const userService = {
  getUsersByDevices: async (deviceIds?: string[]) => {
    const response = await apiService.post<AppResponse<User[]>>('/api/users/devices', { deviceIds })
    if(response.isSuccess){
      return response.data;
    }

    return Promise.reject(new Error(response.errors.join(', ')));
  },
  
  getById: (id: string) => apiService.get<User>(`/api/users/${id}`),
  
  getByPin: (pin: string) => apiService.get<User>(`/api/users/pin/${pin}`),
  
  create: (data: CreateUserRequest) => 
    apiService.post<User>('/api/users', data),
  
  update: (id: string, data: Partial<User>) => 
    apiService.put<void>(`/api/users/${id}`, data),
  
  delete: (id: string) => apiService.delete(`/api/users/${id}`),
  
  syncToDevice: (userId: string, deviceId: string) => 
    apiService.post(`/api/users/${userId}/sync-to-device/${deviceId}`),
  
  syncToAllDevices: (userId: string) => 
    apiService.post(`/api/users/${userId}/sync-to-all-devices`),
  
  getDeviceMappings: (userId: string) => 
    apiService.get<UserDeviceMapping[]>(`/api/users/${userId}/device-mappings`),
};