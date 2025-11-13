
// ==========================================
// src/services/deviceService.ts
// ==========================================
import { apiService } from './api';
import type { Device, CreateDeviceRequest, DeviceCommand, AppResponse, PaginatedResponse, DeviceInfo } from '@/types';
import { DeviceCommandRequest } from '@/types/device';

export const deviceService = {
  getAll: async () => {
    return  await apiService.get<PaginatedResponse<Device>>('/api/devices')
  },

  getByUserId: async (userId: string) => {
    return await apiService.get<Device[]>(`/api/devices/users/${userId}`)
  },
  
  getById: (id: string) => apiService.get<Device>(`/api/devices/${id}`),
  
  create: async (data: CreateDeviceRequest) => {
    return await apiService.post<Device>('/api/devices', data);
  },
  
  delete: (id: string) => apiService.delete<boolean>(`/api/devices/${id}`),

  activeDevice: (id: string) => 
    apiService.put<Device>(`/api/devices/${id}/toggle-active`),
  
  getPendingCommands: (deviceId: string) => 
    apiService.get<DeviceCommand[]>(`/api/devices/${deviceId}/commands`),
  
  sendCommand: (deviceId: string, data: DeviceCommandRequest) => 
    apiService.post<DeviceCommand>(`/api/devices/${deviceId}/commands`, data),

  getDeviceInfo: async (deviceId: string) => {
    return await apiService.get<DeviceInfo>(`/api/devices/${deviceId}/device-info`);
    
  },

  // Device Command Actions
  syncUsers: async (deviceId: string) => {
    return await apiService.post<AppResponse<DeviceCommand>>(
      `/api/devices/${deviceId}/commands`, 
      { command: 'DATA QUERY USERINFO', priority: 1 }
    );
    
  },

  syncAttendance: async (deviceId: string) => {
    return await apiService.post<AppResponse<DeviceCommand>>(
      `/api/devices/${deviceId}/commands`, 
      { command: 'DATA QUERY ATTLOG', priority: 1 }
    );

  },

  clearAttendance: async (deviceId: string) => {
    return await apiService.post<AppResponse<DeviceCommand>>(
      `/api/devices/${deviceId}/commands`, 
      { command: 'DATA DELETE ATTLOG', priority: 1 }
    );
  },

  restartDevice: async (deviceId: string) => {
    return  await apiService.post<AppResponse<DeviceCommand>>(
      `/api/devices/${deviceId}/commands`, 
      { command: 'RESTART', priority: 10 }
    );
   
  },

  lockDevice: async (deviceId: string) => {
    return  await apiService.post<AppResponse<DeviceCommand>>(
      `/api/devices/${deviceId}/commands`, 
      { command: 'CHECK Lock', priority: 5 }
    );
    
  },

  unlockDevice: async (deviceId: string) => {
    return  await apiService.post<AppResponse<DeviceCommand>>(
      `/api/devices/${deviceId}/commands`, 
      { command: 'CHECK Unlock', priority: 5 }
    );
  },
};