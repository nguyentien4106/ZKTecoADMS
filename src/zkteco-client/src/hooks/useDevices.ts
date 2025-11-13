// ==========================================
// src/hooks/useDevices.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { deviceService } from '@/services/deviceService';
import { toast } from 'sonner';
import type { CreateDeviceRequest, Device, SendCommandRequest } from '@/types';
import { DeviceCommandRequest } from '@/types/device';
import { useAuth } from '@/contexts/AuthContext';

export const useDevices = () => {
  return useQuery({
    queryKey: ['devices'],
    queryFn: deviceService.getAll,
  });
};

export const useDevicesByUser = () => {
  const { applicationUserId } = useAuth()

  return useQuery({
    queryKey: ['devices', 'user', applicationUserId],
    queryFn: () => {
      if (!applicationUserId) throw new Error('User ID is required');
      return deviceService.getByUserId(applicationUserId);
    },
    enabled: Boolean(applicationUserId),
    gcTime: 0, // Immediately garbage collect
    retry: false, // Don't retry on error
    staleTime: 5000, // Consider data stale after 5 seconds
  });
}

export const useActiveDevice = () => {
  const { applicationUserId } = useAuth();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (deviceId: string) => deviceService.activeDevice(deviceId),
    onSuccess: (updatedDevice: Device) => {
      console.log('Updated Device:', updatedDevice);
      // Update the specific device in cache
      queryClient.setQueryData(
        ['devices', 'user', applicationUserId],
        (oldData: Device[] | undefined) => {
          if (!oldData) return [updatedDevice];
          return oldData.map(device => 
            device.id === updatedDevice.id ? updatedDevice : device
          );
        }
      );
      
      toast.success('Device status updated successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to update device status', { description: error.message });
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
  const { applicationUserId } = useAuth();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateDeviceRequest) => deviceService.create(data),
    onSuccess: (newDevice: Device) => {
      // Update the user's devices cache directly
      queryClient.setQueryData(
        ['devices', 'user', applicationUserId],
        (oldData: Device[] | undefined) => {
          if (!oldData) return [newDevice];
          return [...oldData, newDevice];
        }
      );
      
      toast.success('Device created successfully');
    },
    onError: (error: any) => {
      toast.error('Add device failed!', { description: error.message });
    }
  });
};

export const useDeleteDevice = () => {
  const { applicationUserId } = useAuth();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => deviceService.delete(id),
    onSuccess: (_data, deletedId) => {
      // Remove the deleted device from cache directly
      queryClient.setQueryData(
        ['devices', 'user', applicationUserId],
        (oldData: Device[] | undefined) => {
          if (!oldData) return [];
          return oldData.filter(device => device.id !== deletedId);
        }
      );
      
      toast.success('Device deleted successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to delete device', { description: error.message });
    }
  });
};

export const useSendCommand = () => {
  return useMutation({
    mutationFn: ({ deviceId, data }: { deviceId: string; data: DeviceCommandRequest }) =>
      deviceService.sendCommand(deviceId, data),
    onSuccess: () => {
      toast.success('Command sent successfully');
    },
    onError: (error: any) => {
      toast.error('Failed to send command', { description: error.message })
    }
  });
};

export const useDevicePendingCommands = (deviceId: string) => {
  return useQuery({
    queryKey: ['commands', deviceId],
    queryFn: () => deviceService.getPendingCommands(deviceId),
    enabled: !!deviceId,
    refetchInterval: 10000, // Refetch every 10 seconds
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

// // Device Command Actions Hooks
// export const useSyncUsers = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (deviceId: string) => deviceService.syncUsers(deviceId),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: ['commands'] });
//     },
//   });
// };

// export const useSyncAttendance = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (deviceId: string) => deviceService.syncAttendance(deviceId),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: ['commands'] });
//     },
//   });
// };

// export const useClearAttendance = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (deviceId: string) => deviceService.clearAttendance(deviceId),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: ['commands'] });
//     },
//   });
// };

// export const useRestartDevice = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (deviceId: string) => deviceService.restartDevice(deviceId),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: ['commands'] });
//     },
//   });
// };

// export const useLockDevice = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (deviceId: string) => deviceService.lockDevice(deviceId),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: ['commands'] });
//     },
//   });
// };

// export const useUnlockDevice = () => {
//   const queryClient = useQueryClient();

//   return useMutation({
//     mutationFn: (deviceId: string) => deviceService.unlockDevice(deviceId),
//     onSuccess: () => {
//       queryClient.invalidateQueries({ queryKey: ['commands'] });
//     },
//   });
// };
