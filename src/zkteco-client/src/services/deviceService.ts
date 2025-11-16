
// ==========================================
// src/services/deviceService.ts
// ==========================================
import { apiService } from './api';
import type { Device, CreateDeviceRequest, PaginatedResponse, DeviceInfo, PaginationRequest } from '@/types';

export const deviceService = {
  getAll: async (request?: PaginationRequest) => {
    return  await apiService.get<PaginatedResponse<Device>>('/api/devices', request)
  },

  getByUserId: async (userId: string) => {
    return await apiService.get<Device[]>(`/api/devices/users/${userId}`)
  },
  
  getById: (id: string) => apiService.get<Device>(`/api/devices/${id}`),
  
  create: async (data: CreateDeviceRequest) => {
    return await apiService.post<Device>('/api/devices', data);
  },
  
  delete: (id: string) => apiService.delete<boolean>(`/api/devices/${id}`),

  toggleActive: (id: string) => 
    apiService.put<Device>(`/api/devices/${id}/toggle-active`),
  
  getDeviceInfo: async (deviceId: string) => {
    return await apiService.get<DeviceInfo>(`/api/devices/${deviceId}/device-info`);
  },
};