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

export const useDevice = (id: number) => {
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
  });
};

export const useDeleteDevice = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => deviceService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['devices'] });
      toast.success('Device deleted successfully');
    },
  });
};

export const useSendCommand = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ deviceId, data }: { deviceId: number; data: SendCommandRequest }) =>
      deviceService.sendCommand(deviceId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['commands'] });
      toast.success('Command sent successfully');
    },
  });
};

export const useDeviceCommands = (deviceId: number) => {
  return useQuery({
    queryKey: ['commands', deviceId],
    queryFn: () => deviceService.getPendingCommands(deviceId),
    enabled: !!deviceId,
    refetchInterval: 10000, // Refetch every 10 seconds
  });
};