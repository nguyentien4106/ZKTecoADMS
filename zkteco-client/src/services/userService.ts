
// ==========================================
// src/services/userService.ts
// ==========================================
import { apiService } from './api';
import type { User, CreateUserRequest, UserDeviceMapping } from '@/types';

export const userService = {
  getAll: () => apiService.get<User[]>('/api/user'),
  
  getById: (id: number) => apiService.get<User>(`/api/user/${id}`),
  
  getByPin: (pin: string) => apiService.get<User>(`/api/user/pin/${pin}`),
  
  create: (data: CreateUserRequest) => 
    apiService.post<User>('/api/user', data),
  
  update: (id: number, data: Partial<User>) => 
    apiService.put<void>(`/api/user/${id}`, data),
  
  delete: (id: number) => apiService.delete(`/api/user/${id}`),
  
  syncToDevice: (userId: number, deviceId: number) => 
    apiService.post(`/api/user/${userId}/sync-to-device/${deviceId}`),
  
  syncToAllDevices: (userId: number) => 
    apiService.post(`/api/user/${userId}/sync-to-all-devices`),
  
  getDeviceMappings: (userId: number) => 
    apiService.get<UserDeviceMapping[]>(`/api/user/${userId}/device-mappings`),
};