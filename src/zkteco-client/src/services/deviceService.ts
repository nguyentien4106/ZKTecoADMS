
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
    return Promise.reject(new Error(response.errors));
  },
  
  getById: (id: number) => apiService.get<Device>(`/api/devices/${id}`),
  
  create: (data: CreateDeviceRequest) => 
    apiService.post<Device>('/api/devices', data),
  
  delete: (id: number) => apiService.delete(`/api/devices/${id}`),
  
  getPendingCommands: (deviceId: number) => 
    apiService.get<DeviceCommand[]>(`/api/devices/${deviceId}/commands`),
  
  sendCommand: (deviceId: number, data: SendCommandRequest) => 
    apiService.post<DeviceCommand>(`/api/devices/${deviceId}/commands`, data),
};