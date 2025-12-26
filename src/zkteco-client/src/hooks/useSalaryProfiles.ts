import { useState, useEffect } from 'react';
import { toast } from 'sonner';
import salaryProfileService, {
  SalaryProfile,
  CreateSalaryProfileRequest,
  UpdateSalaryProfileRequest,
} from '@/services/salaryProfileService';

export const useSalaryProfiles = (showActiveOnly: boolean = false) => {
  const [profiles, setProfiles] = useState<SalaryProfile[]>([]);
  const [loading, setLoading] = useState(true);

  const loadProfiles = async () => {
    try {
      setLoading(true);
      const data = await salaryProfileService.getAllProfiles(showActiveOnly);
      setProfiles(data);
    } catch (error) {
      toast.error('Failed to load salary profiles');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadProfiles();
  }, [showActiveOnly]);

  const createProfile = async (data: CreateSalaryProfileRequest) => {
    try {
      await salaryProfileService.createProfile(data);
      toast.success('Salary profile created successfully');
      await loadProfiles();
    } catch (error: any) {
      toast.error(error.message || 'Failed to create salary profile');
      throw error;
    }
  };

  const updateProfile = async (id: string, data: UpdateSalaryProfileRequest) => {
    try {
      await salaryProfileService.updateProfile(id, data);
      toast.success('Salary profile updated successfully');
      await loadProfiles();
    } catch (error: any) {
      toast.error(error.message || 'Failed to update salary profile');
      throw error;
    }
  };

  const deleteProfile = async (id: string) => {
    if (!confirm('Are you sure you want to delete this salary profile?')) {
      return;
    }

    try {
      await salaryProfileService.deleteProfile(id);
      toast.success('Salary profile deleted successfully');
      await loadProfiles();
    } catch (error: any) {
      toast.error(error.message || 'Failed to delete salary profile');
      throw error;
    }
  };

  return {
    profiles,
    loading,
    createProfile,
    updateProfile,
    deleteProfile,
    refreshProfiles: loadProfiles,
  };
};
