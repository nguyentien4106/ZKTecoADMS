
// ==========================================
// src/services/deviceService.ts
// ==========================================
import { toast } from 'sonner';
import { apiService } from './api';
import type { Device, CreateDeviceRequest, DeviceCommand, SendCommandRequest, AppResponse, PaginatedResponse } from '@/types';

export const deviceService = {
  getAll: async () => {
    const response = await apiService.get<AppResponse<PaginatedResponse<Device>>>('/api/devices')
    if(response.isSuccess){
      return response.data;
    }
    
    toast.error('Failed to fetch devices', { description: response.errors.join(', ') });
    return Promise.reject(new Error(response.errors.join(', ')));
  },

  getByUserId: async (userId: string) => {
    const response = await apiService.get<AppResponse<Device[]>>(`/api/devices/users/${userId}`)
    if(response.isSuccess){
      return response.data;
    }
    
    toast.error('Failed to fetch devices for user', { description: response.errors.join(', ') });
    return Promise.reject(new Error(response.errors.join(', ')));
  },
  
  getById: (id: string) => apiService.get<Device>(`/api/devices/${id}`),
  
  create: async (data: CreateDeviceRequest) => {
    const result = await apiService.post<AppResponse<Device>>('/api/devices', data);
    if(result.isSuccess){
      return result;
    }
    
    return Promise.reject(new Error(result.errors.join(', ')));
  },
  
  delete: (id: string) => apiService.delete(`/api/devices/${id}`),

  activeDevice: (id: string) => 
    apiService.put<Device>(`/api/devices/${id}/toggle-active`),
  
  getPendingCommands: (deviceId: string) => 
    apiService.get<DeviceCommand[]>(`/api/devices/${deviceId}/commands`),
  
  sendCommand: (deviceId: string, data: SendCommandRequest) => 
    apiService.post<DeviceCommand>(`/api/devices/${deviceId}/commands`, data),
};