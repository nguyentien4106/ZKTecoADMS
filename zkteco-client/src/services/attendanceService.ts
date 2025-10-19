
// ==========================================
// src/services/attendanceService.ts
// ==========================================
import { apiService } from './api';
import type { AttendanceLog } from '@/types';

export const attendanceService = {
  getByDevice: (deviceId: number, startDate?: string, endDate?: string) => {
    const params = { startDate, endDate };
    return apiService.get<AttendanceLog[]>(`/api/attendance/device/${deviceId}`, params);
  },
  
  getByUser: (userId: number, startDate?: string, endDate?: string) => {
    const params = { startDate, endDate };
    return apiService.get<AttendanceLog[]>(`/api/attendance/user/${userId}`, params);
  },
  
  getUnprocessed: () => 
    apiService.get<AttendanceLog[]>('/api/attendance/unprocessed'),
  
  markAsProcessed: (logIds: number[]) => 
    apiService.post('/api/attendance/mark-processed', { logIds }),
};