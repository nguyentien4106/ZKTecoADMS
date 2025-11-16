// ==========================================
// src/hooks/useDevices.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { deviceService } from '@/services/deviceService';
import { toast } from 'sonner';
import type { CreateDeviceRequest, Device, PaginatedResponse } from '@/types';

export const useDevices = () => {
  return useQuery({
    queryKey: ['devices'],
    queryFn: () => deviceService.getAll({
      pageNumber: 1,
      pageSize: 1000,
    }),
  });
};

export const useToggleActive = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (deviceId: string) => deviceService.toggleActive(deviceId),
    onSuccess: (updatedDevice: Device) => {
      // Update the specific device in cache
      console.log('Updated Device:', updatedDevice);
      queryClient.setQueryData(
        ['devices'],
        (oldData: PaginatedResponse<Device>) => {
          if (!oldData.items) return [updatedDevice];
          oldData.items = oldData.items.map(device => 
            device.id === updatedDevice.id ? updatedDevice : device
          );

          return oldData;
        }
      );
      
      toast.success(`Device: ${updatedDevice.deviceName} ${updatedDevice.isActive ? 'activated' : 'deactivated'} successfully`);
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
    onSuccess: (newDevice: Device) => {
      // Update the user's devices cache directly
      queryClient.setQueryData(
        ['devices'],
        (oldData: Device[] | undefined) => {
          if (!oldData) return [newDevice];
          return [...oldData, newDevice];
        }
      );
      
      toast.success('Device created successfully');
    }
  });
};

export const useDeleteDevice = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => deviceService.delete(id),
    onSuccess: (_data, deletedId) => {
      queryClient.setQueryData(
        ['devices'],
        (oldData: PaginatedResponse<Device> | undefined) => {
          if (!oldData) return { items: [] };
          return {
            ...oldData,
            items: oldData.items.filter(device => device.id !== deletedId)
          };
        }
      );
      
      toast.success('Device deleted successfully');
    }
  });
};

export const useDeviceInfo = (deviceId: string | null) => {
  return useQuery({
    queryKey: ['deviceInfo', deviceId],
    queryFn: () => {
      if (!deviceId) throw new Error('Device ID is required');
      return deviceService.getDeviceInfo(deviceId);
    },
    enabled: Boolean(deviceId),
    staleTime: 30000, // Consider data stale after 30 seconds
  });
};
