
// ==========================================
// src/hooks/useAttendance.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { attendanceService } from '@/services/attendanceService';
import { toast } from 'sonner';

export const useAttendanceByDevice = (
  deviceId: number,
  startDate?: string,
  endDate?: string
) => {
  return useQuery({
    queryKey: ['attendance', 'device', deviceId, startDate, endDate],
    queryFn: () => attendanceService.getByDevices(deviceId, startDate, endDate),
    enabled: !!deviceId,
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

export const useUnprocessedAttendance = () => {
  return useQuery({
    queryKey: ['attendance', 'unprocessed'],
    queryFn: attendanceService.getUnprocessed,
    refetchInterval: 30000, // Refetch every 30 seconds
  });
};

export const useMarkAttendanceAsProcessed = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (logIds: number[]) => attendanceService.markAsProcessed(logIds),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['attendance'] });
      toast.success('Attendance logs marked as processed');
    },
  });
};