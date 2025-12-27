import { createContext, useContext, useState, ReactNode } from 'react';
import {
  useSalaryProfiles,
  useCreateSalaryProfile,
  useUpdateSalaryProfile,
  useDeleteSalaryProfile,
} from '@/hooks/useSalaryProfiles';
import {
  SalaryProfile,
  CreateSalaryProfileRequest,
  UpdateSalaryProfileRequest,
} from '@/services/salaryProfileService';

interface SalaryProfileContextValue {
  // State
  profiles: SalaryProfile[] | undefined;
  isLoading: boolean;
  showActiveOnly: boolean;
  
  // Dialog states
  createDialogOpen: boolean;
  editDialogOpen: boolean;
  assignDialogOpen: boolean;
  profileToEdit: SalaryProfile | null;
  profileToAssign: SalaryProfile | null;
  
  // Actions
  setCreateDialogOpen: (open: boolean) => void;
  setEditDialogOpen: (open: boolean) => void;
  setAssignDialogOpen: (open: boolean) => void;
  setShowActiveOnly: (show: boolean) => void;
  handleCreateProfile: (data: CreateSalaryProfileRequest) => Promise<void>;
  handleUpdateProfile: (data: UpdateSalaryProfileRequest) => Promise<void>;
  handleEdit: (profile: SalaryProfile) => void;
  handleOpenCreateDialog: () => void;
  handleDelete: (id: string) => Promise<void>;
  handleOpenAssignDialog: (profile: SalaryProfile) => void;
  
  // Mutation states
  isCreatePending: boolean;
  isUpdatePending: boolean;
  isDeletePending: boolean;
}

const SalaryProfileContext = createContext<SalaryProfileContextValue | undefined>(undefined);

export const useSalaryProfileContext = () => {
  const context = useContext(SalaryProfileContext);
  if (!context) {
    throw new Error('useSalaryProfileContext must be used within SalaryProfileProvider');
  }
  return context;
};

interface SalaryProfileProviderProps {
  children: ReactNode;
}

export const SalaryProfileProvider = ({ children }: SalaryProfileProviderProps) => {
  // State
  const [showActiveOnly, setShowActiveOnly] = useState(false);
  const [createDialogOpen, setCreateDialogOpenState] = useState(false);
  const [editDialogOpen, setEditDialogOpenState] = useState(false);
  const [assignDialogOpen, setAssignDialogOpenState] = useState(false);
  const [profileToEdit, setProfileToEdit] = useState<SalaryProfile | null>(null);
  const [profileToAssign, setProfileToAssign] = useState<SalaryProfile | null>(null);
  
  // Hooks
  const { data: profiles, isLoading } = useSalaryProfiles(showActiveOnly);
  const createProfileMutation = useCreateSalaryProfile();
  const updateProfileMutation = useUpdateSalaryProfile();
  const deleteProfileMutation = useDeleteSalaryProfile();

  // Wrapper to clear profile state when closing dialog
  const setCreateDialogOpen = (open: boolean) => {
    if (!open) {
      setProfileToEdit(null);
    }
    setCreateDialogOpenState(open);
  };

  const setEditDialogOpen = (open: boolean) => {
    if (!open) {
      setProfileToEdit(null);
    }
    setEditDialogOpenState(open);
  };

  const setAssignDialogOpen = (open: boolean) => {
    if (!open) {
      setProfileToAssign(null);
    }
    setAssignDialogOpenState(open);
  };

  // Handlers
  const handleCreateProfile = async (data: CreateSalaryProfileRequest) => {
    await createProfileMutation.mutateAsync(data);
    setCreateDialogOpen(false);
  };

  const handleUpdateProfile = async (data: UpdateSalaryProfileRequest) => {
    if (!profileToEdit?.id) return;
    
    await updateProfileMutation.mutateAsync({
      id: profileToEdit.id,
      data,
    });
    setEditDialogOpen(false);
    setProfileToEdit(null);
  };

  const handleEdit = (profile: SalaryProfile) => {
    setProfileToEdit(profile);
    setEditDialogOpen(true);
  };

  const handleOpenCreateDialog = () => {
    setProfileToEdit(null);
    setCreateDialogOpen(true);
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this salary profile?')) {
      return;
    }
    await deleteProfileMutation.mutateAsync(id);
  };

  const handleOpenAssignDialog = (profile: SalaryProfile) => {
    setProfileToAssign(profile);
    setAssignDialogOpen(true);
  };

  const value: SalaryProfileContextValue = {
    // State
    profiles,
    isLoading,
    showActiveOnly,
    
    // Dialog states
    createDialogOpen,
    editDialogOpen,
    assignDialogOpen,
    profileToEdit,
    profileToAssign,
    
    // Actions
    setCreateDialogOpen,
    setEditDialogOpen,
    setAssignDialogOpen,
    setShowActiveOnly,
    handleCreateProfile,
    handleUpdateProfile,
    handleEdit,
    handleOpenCreateDialog,
    handleDelete,
    handleOpenAssignDialog,
    
    // Mutation states
    isCreatePending: createProfileMutation.isPending,
    isUpdatePending: updateProfileMutation.isPending,
    isDeletePending: deleteProfileMutation.isPending,
  };

  return (
    <SalaryProfileContext.Provider value={value}>
      {children}
    </SalaryProfileContext.Provider>
  );
};
