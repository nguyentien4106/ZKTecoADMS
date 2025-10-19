
// ==========================================
// src/services/deviceService.ts
// ==========================================
import { apiService } from './api';
import type { Device, CreateDeviceRequest, DeviceCommand, SendCommandRequest } from '@/types';

export const deviceService = {
  getAll: () => apiService.get<Device[]>('/api/device'),
  
  getById: (id: number) => apiService.get<Device>(`/api/device/${id}`),
  
  create: (data: CreateDeviceRequest) => 
    apiService.post<Device>('/api/device', data),
  
  delete: (id: number) => apiService.delete(`/api/device/${id}`),
  
  getPendingCommands: (deviceId: number) => 
    apiService.get<DeviceCommand[]>(`/api/device/${deviceId}/commands`),
  
  sendCommand: (deviceId: number, data: SendCommandRequest) => 
    apiService.post<DeviceCommand>(`/api/device/${deviceId}/commands`, data),
};