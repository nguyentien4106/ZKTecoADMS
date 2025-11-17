// ==========================================
// src/contexts/ShiftContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode } from 'react';
import { Shift, CreateShiftRequest, UpdateShiftRequest } from '@/types/shift';
import { useAuth } from '@/contexts/AuthContext';
import { 
  useMyShifts, 
  useCreateShift, 
  useUpdateShift, 
  useDeleteShift 
} from '@/hooks/useShifts';

interface ShiftContextValue {
  // State
  shifts: Shift[];
  isLoading: boolean;
  
  // Dialog states
  createDialogOpen: boolean;
  updateDialogOpen: boolean;
  selectedShift: Shift | null;
  
  // Actions
  setCreateDialogOpen: (open: boolean) => void;
  setUpdateDialogOpen: (open: boolean) => void;
  handleCreate: (data: CreateShiftRequest) => Promise<void>;
  handleUpdate: (id: string, data: UpdateShiftRequest) => Promise<void>;
  handleDelete: (id: string) => Promise<void>;
  handleEdit: (shift: Shift) => void;
}

const ShiftContext = createContext<ShiftContextValue | undefined>(undefined);

export const useShiftContext = () => {
  const context = useContext(ShiftContext);
  if (!context) {
    throw new Error('useShiftContext must be used within ShiftProvider');
  }
  return context;
};

interface ShiftProviderProps {
  children: ReactNode;
}

export const ShiftProvider = ({ children }: ShiftProviderProps) => {
  // Dialog states
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [updateDialogOpen, setUpdateDialogOpen] = useState(false);
  const [selectedShift, setSelectedShift] = useState<Shift | null>(null);

  // Get current user

  // Hooks
  const { data: shifts = [], isLoading } = useMyShifts();
  const createShiftMutation = useCreateShift();
  const updateShiftMutation = useUpdateShift();
  const deleteShiftMutation = useDeleteShift();

  const handleCreate = async (data: CreateShiftRequest) => {
    const createData: CreateShiftRequest = {
      ...data,
    };
    
    await createShiftMutation.mutateAsync(createData);
    setCreateDialogOpen(false);
  };

  const handleUpdate = async (id: string, data: UpdateShiftRequest) => {
    await updateShiftMutation.mutateAsync({ id, data });
    setUpdateDialogOpen(false);
    setSelectedShift(null);
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this shift?')) {
      return;
    }

    await deleteShiftMutation.mutateAsync(id);
  };

  const handleEditClick = (shift: Shift) => {
    setSelectedShift(shift);
    setUpdateDialogOpen(true);
  };

  const value: ShiftContextValue = {
    // State
    shifts,
    isLoading,
    
    // Dialog states
    createDialogOpen,
    updateDialogOpen,
    selectedShift,
    
    // Actions
    setCreateDialogOpen,
    setUpdateDialogOpen,
    handleCreate,
    handleUpdate,
    handleDelete,
    handleEdit: handleEditClick,
  };

  return (
    <ShiftContext.Provider value={value}>
      {children}
    </ShiftContext.Provider>
  );
};
