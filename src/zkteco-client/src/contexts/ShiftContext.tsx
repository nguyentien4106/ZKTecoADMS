// ==========================================
// src/contexts/ShiftContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode, Dispatch, SetStateAction, useCallback } from 'react';
import { Shift, CreateShiftRequest, UpdateShiftRequest, ShiftStatus } from '@/types/shift';
import { 
  useMyShifts, 
  useCreateShift, 
  useUpdateShift, 
  useDeleteShift 
} from '@/hooks/useShifts';

interface ShiftContextValue {
  // State
  isLoading: boolean;
  shifts: Shift[];

  // Filter state
  selectedMonth: Date;
  selectedStatus: ShiftStatus | 'all';
  setSelectedMonth: Dispatch<SetStateAction<Date>>;
  setSelectedStatus: Dispatch<SetStateAction<ShiftStatus | 'all'>>;
  applyFilters: () => void;
  clearFilters: () => void;

  // Dialog states
  dialogMode: 'create' | 'edit' | null;
  setDialogMode: (mode: 'create' | 'edit' | null) => void;
  selectedShift: Shift | null;
  
  // Actions
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
  const [dialogMode, setDialogMode] = useState<'create' | 'edit' | null>(null);
  const [selectedShift, setSelectedShift] = useState<Shift | null>(null);
  
  // Filter states
  const currentDate = new Date();
  const [selectedMonth, setSelectedMonth] = useState<Date>(currentDate);
  const [selectedStatus, setSelectedStatus] = useState<ShiftStatus | 'all'>('all');
  const [appliedMonth, setAppliedMonth] = useState<Date>(currentDate);
  const [appliedStatus, setAppliedStatus] = useState<ShiftStatus | 'all'>('all');

  // Hooks
  const { data: shifts, isLoading } = useMyShifts(
    appliedMonth.getMonth() + 1, // getMonth() returns 0-11, backend expects 1-12
    appliedMonth.getFullYear(),
    appliedStatus
  );
  const createShiftMutation = useCreateShift();
  const updateShiftMutation = useUpdateShift();
  const deleteShiftMutation = useDeleteShift();

  const applyFilters = useCallback(() => {
    setAppliedMonth(selectedMonth);
    setAppliedStatus(selectedStatus);
  }, [selectedMonth, selectedStatus]);

  const clearFilters = useCallback(() => {
    const now = new Date();
    setSelectedMonth(now);
    setSelectedStatus('all');
    setAppliedMonth(now);
    setAppliedStatus('all');
  }, []);

  const handleCreate = useCallback(async (data: CreateShiftRequest) => {
    await createShiftMutation.mutateAsync(data);
    setDialogMode(null);
  }, [createShiftMutation]);

  const handleUpdate = useCallback(async (id: string, data: UpdateShiftRequest) => {
    await updateShiftMutation.mutateAsync({ id, data });
    setSelectedShift(null);
    setDialogMode(null);
  }, [updateShiftMutation]);

  const handleDelete = useCallback(async (id: string) => {
    await deleteShiftMutation.mutateAsync(id);
  }, [deleteShiftMutation]);

  const handleEdit = useCallback((shift: Shift) => {
    setSelectedShift(shift);
    setDialogMode('edit');
  }, []);

  // Memoize the context value
  const value: ShiftContextValue = {
    // State
    shifts: shifts || [],
    isLoading,
    
    // Filter state
    selectedMonth,
    selectedStatus,
    setSelectedMonth,
    setSelectedStatus,
    applyFilters,
    clearFilters,
    
    // Dialog states
    dialogMode,
    setDialogMode,
    selectedShift,
    
    // Actions
    handleCreate,
    handleUpdate,
    handleDelete,
    handleEdit,
  }

  return (
    <ShiftContext.Provider value={value}>
      {children}
    </ShiftContext.Provider>
  );
};
