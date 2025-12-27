import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import salaryProfileService, {
  AssignSalaryProfileRequest,
  CreateSalaryProfileRequest,
  UpdateSalaryProfileRequest,
} from '@/services/salaryProfileService';
import { toast } from 'sonner';

export const useSalaryProfiles = (showActiveOnly: boolean = false) => {
  return useQuery({
    queryKey: ['salaryProfiles', showActiveOnly],
    queryFn: () => salaryProfileService.getAllProfiles(showActiveOnly),
  });
};

export const useSalaryProfileById = (id: string) => {
  return useQuery({
    queryKey: ['salaryProfile', id],
    queryFn: () => salaryProfileService.getProfileById(id),
    enabled: !!id,
  });
};

export const useCreateSalaryProfile = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateSalaryProfileRequest) => salaryProfileService.createProfile(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['salaryProfiles'] });
      toast.success('Salary profile created successfully');
    },
    onError: (error: Error) => {
      toast.error('Failed to create salary profile', {
        description: error.message,
      });
    },
  });
};

export const useUpdateSalaryProfile = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateSalaryProfileRequest }) =>
      salaryProfileService.updateProfile(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['salaryProfiles'] });
      toast.success('Salary profile updated successfully');
    },
    onError: (error: Error) => {
      toast.error('Failed to update salary profile', {
        description: error.message,
      });
    },
  });
};

export const useDeleteSalaryProfile = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => salaryProfileService.deleteProfile(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['salaryProfiles'] });
      toast.success('Salary profile deleted successfully');
    },
    onError: (error: Error) => {
      toast.error('Failed to delete salary profile', {
        description: error.message,
      });
    },
  });
};


export const useAssignSalaryProfile = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: AssignSalaryProfileRequest) => salaryProfileService.assignProfile(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['salaryProfiles'] });
      queryClient.invalidateQueries({ queryKey: ['employeeSalaryProfiles'] });
      toast.success('Salary profile assigned to employee successfully');
    },
    onError: (error: Error) => {
      toast.error('Failed to assign salary profile', {
        description: error.message,
      });
    },
  });
}

export const useEmployeeSalaryProfiles = (params?: { pageNumber?: number; pageSize?: number }) => {
  return useQuery({
    queryKey: ['employeeSalaryProfiles', params],
    queryFn: () => salaryProfileService.getAllEmployeeSalaryProfiles(params),
  });
};
