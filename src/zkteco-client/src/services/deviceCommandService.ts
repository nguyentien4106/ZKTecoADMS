
// ==========================================
// src/services/attendanceService.ts
// ==========================================
import { apiService } from './api';
import type { DeviceCommand } from '@/types';

export const deviceCommandService = {
  getByDevice: (deviceId: string) => {
    return apiService.get<DeviceCommand[]>(`/api/devices/${deviceId}/commands`);
  },
  
};