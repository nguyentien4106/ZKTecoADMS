// ==========================================
// src/hooks/useShifts.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { shiftService } from '@/services/shiftService';
import { toast } from 'sonner';
import { CreateShiftRequest, UpdateShiftRequest, RejectShiftRequest } from '@/types/shift';

// Query hooks
export const useMyShifts = () => {
  return useQuery({
    queryKey: ['shifts', 'my-shifts'],
    queryFn: () => shiftService.getMyShifts(),
  });
};

export const usePendingShifts = () => {
  return useQuery({
    queryKey: ['shifts', 'pending'],
    queryFn: () => shiftService.getPendingShifts(),
  });
};

export const useManagedShifts = () => {
  return useQuery({
    queryKey: ['shifts', 'managed'],
    queryFn: () => shiftService.getManagedShifts(),
  });
};

// Mutation hooks
export const useCreateShift = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateShiftRequest) => shiftService.createShift(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['shifts'] });
      toast.success('Shift created successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to create shift', {
        description: error.message || 'An error occurred',
      });
    },
  });
};

export const useUpdateShift = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateShiftRequest }) =>
      shiftService.updateShift(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['shifts'] });
      toast.success('Shift updated successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to update shift', {
        description: error.message || 'An error occurred',
      });
    },
  });
};

export const useDeleteShift = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => shiftService.deleteShift(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['shifts'] });
      toast.success('Shift deleted successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to delete shift', {
        description: error.message || 'An error occurred',
      });
    },
  });
};

export const useApproveShift = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => shiftService.approveShift(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['shifts'] });
      toast.success('Shift approved successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to approve shift', {
        description: error.message || 'An error occurred',
      });
    },
  });
};

export const useRejectShift = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: RejectShiftRequest }) =>
      shiftService.rejectShift(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['shifts'] });
      toast.success('Shift rejected successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to reject shift', {
        description: error.message || 'An error occurred',
      });
    },
  });
};
