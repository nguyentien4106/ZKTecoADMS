// ==========================================
// src/hooks/useDevices.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { deviceService } from '@/services/deviceService';
import { toast } from 'sonner';
import type { CreateDeviceRequest, SendCommandRequest } from '@/types';

export const useDevices = () => {
  return useQuery({
    queryKey: ['devices'],
    queryFn: deviceService.getAll,
  });
};

export const useDevicesByUser = (userId: string | null) => {
  return useQuery({
    queryKey: ['devices', 'user', userId],
    queryFn: () => {
      if (!userId) throw new Error('User ID is required');
      return deviceService.getByUserId(userId);
    },
    enabled: Boolean(userId),
    gcTime: 0, // Immediately garbage collect
    retry: false, // Don't retry on error
    staleTime: 5000, // Consider data stale after 5 seconds
  });
}

export const useDevicesOnline = () => {
  return useQuery({
    queryKey: ['devices', 'online'],
    queryFn: async () => {
      const allDevices = await deviceService.getAll();
      return allDevices.items.filter(device => device.deviceStatus === 'Online');
    },
  });
}

export const useActiveDevice = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (deviceId: string) => deviceService.activeDevice(deviceId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['devices'] });
      toast.success('Device created successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to activate device', { description: error.message });
    }
  });
}

export const useDevice = (id: string) => {
  return useQuery({
    queryKey: ['device', id],
    queryFn: () => deviceService.getById(id),
    enabled: !!id,
  });
};

export const useCreateDevice = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateDeviceRequest) => deviceService.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['devices'] });
      toast.success('Device created successfully');
    },
    onError: (error: any) => {
      toast.error('Add device failed!', { description: error.message });
    }
  });
};

export const useDeleteDevice = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => deviceService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['devices'] });
      toast.success('Device deleted successfully');
    },
  });
};

export const useSendCommand = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ deviceId, data }: { deviceId: string; data: SendCommandRequest }) =>
      deviceService.sendCommand(deviceId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['commands'] });
      toast.success('Command sent successfully');
    },
  });
};

export const useDeviceCommands = (deviceId: string) => {
  return useQuery({
    queryKey: ['commands', deviceId],
    queryFn: () => deviceService.getPendingCommands(deviceId),
    enabled: !!deviceId,
    refetchInterval: 10000, // Refetch every 10 seconds
  });
};