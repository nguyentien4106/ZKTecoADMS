// ==========================================
// src/contexts/ShiftContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode } from 'react';
import { Shift, CreateShiftRequest, UpdateShiftRequest } from '@/types/shift';
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
  dialogMode: 'create' | 'edit' | null;
  setDialogMode: (mode: 'create' | 'edit' | null) => void;
  selectedShift: Shift | null;
  
  // Actions
  handleCreateOrUpdate: (data: CreateShiftRequest | UpdateShiftRequest, id?: string) => Promise<void>;
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
  const [dialogMode, setDialogMode] = useState<'create' | 'edit' | null>(null);
  const [selectedShift, setSelectedShift] = useState<Shift | null>(null);

  // Hooks
  const { data: shifts = [], isLoading } = useMyShifts();
  const createShiftMutation = useCreateShift();
  const updateShiftMutation = useUpdateShift();
  const deleteShiftMutation = useDeleteShift();

  const handleCreateOrUpdate = async (data: CreateShiftRequest | UpdateShiftRequest, id?: string) => {
    if (dialogMode === 'edit' && id) {
      await updateShiftMutation.mutateAsync({ id, data: data as UpdateShiftRequest });
      setSelectedShift(null);
    } else {
      await createShiftMutation.mutateAsync(data as CreateShiftRequest);
    }
    setDialogMode(null);
  };

  const handleDelete = async (id: string) => {
    await deleteShiftMutation.mutateAsync(id);
  };

  const handleEdit = (shift: Shift) => {
    setSelectedShift(shift);
    setDialogMode('edit');
  };

  const value: ShiftContextValue = {
    // State
    shifts,
    isLoading,
    
    // Dialog states
    dialogMode,
    setDialogMode,
    selectedShift,
    
    // Actions
    handleCreateOrUpdate,
    handleDelete,
    handleEdit,
  };

  return (
    <ShiftContext.Provider value={value}>
      {children}
    </ShiftContext.Provider>
  );
};
