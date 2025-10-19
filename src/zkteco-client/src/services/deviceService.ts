
// ==========================================
// src/services/deviceService.ts
// ==========================================
import { apiService } from './api';
import type { Device, CreateDeviceRequest, DeviceCommand, SendCommandRequest } from '@/types';

export const deviceService = {
  getAll: () => apiService.get<Device[]>('/api/devices'),
  
  getById: (id: number) => apiService.get<Device>(`/api/devices/${id}`),
  
  create: (data: CreateDeviceRequest) => 
    apiService.post<Device>('/api/devices', data),
  
  delete: (id: number) => apiService.delete(`/api/devices/${id}`),
  
  getPendingCommands: (deviceId: number) => 
    apiService.get<DeviceCommand[]>(`/api/devices/${deviceId}/commands`),
  
  sendCommand: (deviceId: number, data: SendCommandRequest) => 
    apiService.post<DeviceCommand>(`/api/devices/${deviceId}/commands`, data),
};