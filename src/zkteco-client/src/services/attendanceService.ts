
// ==========================================
// src/services/attendanceService.ts
// ==========================================
import { AttendancesFilterParams } from '@/types/attendance';
import { apiService } from './api';
import type { AppResponse, AttendanceLog, PaginatedResponse, PaginationRequest } from '@/types';
const buildQueryString = (params: any) => {
  return Object.entries(params)
    .map(([key, value]) => {
      if (Array.isArray(value)) {
        return value.map(v => `${encodeURIComponent(key)}=${encodeURIComponent(v)}`).join('&');
      }
      return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
    })
    .join('&');
}

export const attendanceService = {
  getByDevices: (paginationRequest: PaginationRequest, filterParams: AttendancesFilterParams) => {

    return apiService.post<AppResponse<PaginatedResponse<AttendanceLog>>>('/api/attendances/devices?' + buildQueryString(paginationRequest), filterParams);
  },
  
  getByUser: (userId: number, startDate?: string, endDate?: string) => {
    const params = { startDate, endDate };
    return apiService.get<AttendanceLog[]>(`/api/attendance/user/${userId}`, params);
  },
  
  getFiltered: (deviceIds?: string[], startDate?: string, endDate?: string) => {
    // For multiple devices, we'll fetch unprocessed and filter on client side for now
    // or you can create a backend endpoint that accepts multiple device IDs
    const params: any = {};
    if (startDate) params.startDate = startDate;
    if (endDate) params.endDate = endDate;
    
    // If only one device, use the device-specific endpoint
    if (deviceIds && deviceIds.length === 1) {
      return apiService.get<AttendanceLog[]>(`/api/attendances/device/${deviceIds[0]}`, params);
    }
    
    // Otherwise get unprocessed and filter client-side
    return apiService.get<AttendanceLog[]>('/api/attendances/unprocessed', params);
  },

};