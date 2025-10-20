
// ==========================================
// src/hooks/useUsers.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { userService } from '@/services/userService';
import { toast } from 'sonner';
import type { CreateUserRequest, User } from '@/types';

export const useUsers = (deviceIds: string[]) => {
  return useQuery({
    queryKey: ['users', deviceIds],
    queryFn: () => userService.getUsersByDevices(deviceIds),
  });
};

export const useUser = (id: string) => {
  return useQuery({
    queryKey: ['user', id],
    queryFn: () => userService.getById(id),
    enabled: !!id,
  });
};

export const useCreateUser = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateUserRequest) => userService.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
      toast.success('User created successfully');
    },
  });
};

export const useUpdateUser = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: Partial<User> }) =>
      userService.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
      toast.success('User updated successfully');
    },
  });
};

export const useDeleteUser = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => userService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
      toast.success('User deleted successfully');
    },
  });
};

export const useSyncUserToDevice = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ userId, deviceId }: { userId: string; deviceId: string }) =>
      userService.syncToDevice(userId, deviceId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['user-mappings'] });
      toast.success('User sync initiated');
    },
  });
};

export const useSyncUserToAllDevices = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (userId: string) => userService.syncToAllDevices(userId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['user-mappings'] });
      toast.success('User sync to all devices initiated');
    },
  });
};

export const useUserDeviceMappings = (userId: string) => {
  return useQuery({
    queryKey: ['user-mappings', userId],
    queryFn: () => userService.getDeviceMappings(userId),
    enabled: !!userId,
  });
};