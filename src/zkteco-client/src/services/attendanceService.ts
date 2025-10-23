
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
      return `${encodeURIComponent(key)}=${encodeURIComponent(value as any)}`;
    })
    .join('&');
}

export const attendanceService = {
  getByDevices: (paginationRequest: PaginationRequest, filterParams: AttendancesFilterParams) => {

    return apiService.post<AppResponse<PaginatedResponse<AttendanceLog>>>('/api/attendances/devices?' + buildQueryString(paginationRequest), filterParams);
  },
  
  getByUser: (userId: number, startDate?: string, endDate?: string) => {
    const params = { startDate, endDate };
    return apiService.get<AttendanceLog[]>(`/api/attendances/users/${userId}`, params);
  }

};