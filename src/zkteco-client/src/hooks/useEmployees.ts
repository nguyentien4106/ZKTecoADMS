
// ==========================================
// src/hooks/useEmployees.ts
// ==========================================
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { employeeService } from '@/services/employeeService';
import { toast } from 'sonner';
import { CreateEmployeeRequest, Employee, UpdateEmployeeRequest } from '@/types/employee';
import { accountService } from '@/services/accountService';
import { CreateEmployeeAccountRequest, UpdateEmployeeAccountRequest } from '@/types/account';
import { AppResponse } from '@/types';

export const useEmployees = (deviceIds: string[]) => {
  return useQuery({
    queryKey: ['employees', deviceIds],
    queryFn: () => employeeService.getEmployeesByDevices(deviceIds),
  });
};

export const useEmployee = (id: string) => {
  return useQuery({
    queryKey: ['employee', id],
    queryFn: () => employeeService.getById(id),
    enabled: !!id,
  });
};

export const useCreateEmployee = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateEmployeeRequest) => employeeService.create(data),
    onSuccess: (results: AppResponse<Employee>[]) => {
      queryClient.setQueryData(
        ['employees'],
        (oldData: Employee[]) => {
          if (!oldData) return;
          const newEmployees = results.filter(res => res.isSuccess).map(res => res.data);
          const failedEmployees = results.filter(res => !res.isSuccess);
          failedEmployees.forEach(failed => {
            toast.error(`Failed to create employee: ${failed.errors.join("\n")}`);
          });
          return [...oldData, ...newEmployees].sort((a, b) => a.pin.localeCompare(b.pin));
        }
      );
      toast.success('Employee created successfully');
    },

  });
};

export const useUpdateEmployee = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateEmployeeRequest) => employeeService.update(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] });
      toast.success('Employee updated successfully');
    },
  });
};

export const useDeleteEmployee = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => employeeService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] });
      toast.success('Employee deleted successfully');
    },
  });
};

export const useCreateEmployeeAccount = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateEmployeeAccountRequest) => accountService.createUserAccount(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] });
      toast.success('Employee account created successfully');
    },
  });
};

export const useUpdateEmployeeAccount = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({employeeDeviceId, data}: {employeeDeviceId: string, data: UpdateEmployeeAccountRequest}) => accountService.updateUserAccount(employeeDeviceId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] });
      toast.success('Employee account updated successfully');
    },
  });
};