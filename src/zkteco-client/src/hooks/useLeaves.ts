import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { leaveService } from '@/services/leaveService';
import { toast } from 'sonner';
import { CreateLeaveRequest, RejectLeaveRequest } from '@/types/leave';

// Query hooks
export const useMyLeaves = () => {
  return useQuery({
    queryKey: ['leaves', 'my-leaves'],
    queryFn: () => leaveService.getMyLeaves(),
  });
};

export const usePendingLeaves = () => {
  return useQuery({
    queryKey: ['leaves', 'pending'],
    queryFn: () => leaveService.getPendingLeaves(),
  });
};

export const useAllLeaves = () => {
  return useQuery({
    queryKey: ['leaves', 'all'],
    queryFn: () => leaveService.getAllLeaves(),
  });
};

// Mutation hooks
export const useCreateLeave = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateLeaveRequest) => leaveService.createLeave(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leaves'] });
      toast.success('Leave request submitted successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to submit leave request', {
        description: error.message || 'An error occurred',
      });
    },
  });
};

export const useCancelLeave = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => leaveService.cancelLeave(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leaves'] });
      toast.success('Leave request cancelled successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to cancel leave request', {
        description: error.message || 'An error occurred',
      });
    },
  });
};

export const useApproveLeave = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => leaveService.approveLeave(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leaves'] });
      toast.success('Leave request approved');
    },
    onError: (error: any) => {
      toast.error('Failed to approve leave request', {
        description: error.message || 'An error occurred',
      });
    },
  });
};

export const useRejectLeave = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: RejectLeaveRequest }) => 
      leaveService.rejectLeave(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leaves'] });
      toast.success('Leave request rejected');
    },
    onError: (error: any) => {
      toast.error('Failed to reject leave request', {
        description: error.message || 'An error occurred',
      });
    },
  });
};
