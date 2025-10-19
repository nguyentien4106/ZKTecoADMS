
// ==========================================
// src/services/usersService.ts
// ==========================================
import { apiService } from './api';
import type { User, CreateUserRequest, UserDeviceMapping } from '@/types';

export const userService = {
  getAll: () => apiService.get<User[]>('/api/users'),
  
  getById: (id: number) => apiService.get<User>(`/api/users/${id}`),
  
  getByPin: (pin: string) => apiService.get<User>(`/api/users/pin/${pin}`),
  
  create: (data: CreateUserRequest) => 
    apiService.post<User>('/api/users', data),
  
  update: (id: number, data: Partial<User>) => 
    apiService.put<void>(`/api/users/${id}`, data),
  
  delete: (id: number) => apiService.delete(`/api/users/${id}`),
  
  syncToDevice: (userId: number, deviceId: number) => 
    apiService.post(`/api/users/${userId}/sync-to-device/${deviceId}`),
  
  syncToAllDevices: (userId: number) => 
    apiService.post(`/api/users/${userId}/sync-to-all-devices`),
  
  getDeviceMappings: (userId: number) => 
    apiService.get<UserDeviceMapping[]>(`/api/users/${userId}/device-mappings`),
};