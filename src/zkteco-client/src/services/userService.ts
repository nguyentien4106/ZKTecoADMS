
// ==========================================
// src/services/usersService.ts
// ==========================================
import { CreateUserRequest, UpdateUserRequest } from '@/types/user';
import { apiService } from './api';
import type { User, AppResponse } from '@/types';

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
  
  create: async (data: CreateUserRequest) => {
    return await apiService.post<AppResponse<User>[]>('/api/users', data)
  },
  
  update: (data: UpdateUserRequest) => 
    apiService.put<AppResponse<User>>(`/api/users/${data.userId}`, data),

  delete: (id: string) => apiService.delete<AppResponse<string>>(`/api/users/${id}`),
  
  syncToDevice: (userId: string, deviceId: string) => 
    apiService.post(`/api/users/${userId}/sync-to-device/${deviceId}`),
};