
// ==========================================
// src/hooks/useAttendance.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { attendanceService } from '@/services/attendanceService';
import { PaginationRequest } from '@/types';
import { AttendancesFilterParams } from '@/types/attendance';

export const useAttendancesByDevices = (
  paginationRequest: PaginationRequest,
  filter: AttendancesFilterParams,
) => {
  return useQuery({
    queryKey: ['attendance', 'devices', filter?.deviceIds, filter?.fromDate, filter?.toDate],
    queryFn: () => attendanceService.getByDevices(paginationRequest, filter),
    enabled: !!filter?.deviceIds?.length && filter.fromDate <= filter.toDate,
  });
};

export const useAttendanceByUser = (
  userId: number,
  startDate?: string,
  endDate?: string
) => {
  return useQuery({
    queryKey: ['attendance', 'user', userId, startDate, endDate],
    queryFn: () => attendanceService.getByUser(userId, startDate, endDate),
    enabled: !!userId,
  });
};


export const useFilteredAttendance = (
  deviceIds?: string[],
  startDate?: string,
  endDate?: string
) => {
  return useQuery({
    queryKey: ['attendance', 'filtered', deviceIds, startDate, endDate],
    queryFn: () => attendanceService.getFiltered(deviceIds, startDate, endDate),
    select: (data) => {
      // Client-side filtering for multiple devices
      if (!deviceIds || deviceIds.length === 0) {
        return data;
      }
      return data.filter((log) => 
        deviceIds.includes(log.device?.id || log.deviceId?.toString() || '')
      );
    },
  });
};
