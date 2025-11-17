
// ==========================================
// src/hooks/useUsers.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { userService } from '@/services/employeeService';
import { toast } from 'sonner';
import { CreateEmployeeRequest, UpdateEmployeeRequest } from '@/types/employee';

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
  return useMutation({
    mutationFn: (data: CreateEmployeeRequest) => userService.create(data),
  });
};

export const useUpdateUser = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateEmployeeRequest) => userService.update(data),
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
