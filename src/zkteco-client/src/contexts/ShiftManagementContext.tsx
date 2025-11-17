// ==========================================
// src/contexts/ShiftManagementContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode } from 'react';
import { Shift } from '@/types/shift';
import { 
  usePendingShifts, 
  useManagedShifts, 
  useApproveShift, 
  useRejectShift 
} from '@/hooks/useShifts';

interface ShiftManagementContextValue {
  // State
  pendingShifts: Shift[];
  allShifts: Shift[];
  isLoading: boolean;
  
  // Dialog states
  approveDialogOpen: boolean;
  rejectDialogOpen: boolean;
  selectedShift: Shift | null;
  
  // Actions
  setApproveDialogOpen: (open: boolean) => void;
  setRejectDialogOpen: (open: boolean) => void;
  handleApprove: (id: string) => Promise<void>;
  handleReject: (id: string, rejectionReason: string) => Promise<void>;
  handleApproveClick: (shift: Shift) => void;
  handleRejectClick: (shift: Shift) => void;
}

const ShiftManagementContext = createContext<ShiftManagementContextValue | undefined>(undefined);

export const useShiftManagementContext = () => {
  const context = useContext(ShiftManagementContext);
  if (!context) {
    throw new Error('useShiftManagementContext must be used within ShiftManagementProvider');
  }
  return context;
};

interface ShiftManagementProviderProps {
  children: ReactNode;
}

export const ShiftManagementProvider = ({ children }: ShiftManagementProviderProps) => {
  // Dialog states
  const [approveDialogOpen, setApproveDialogOpen] = useState(false);
  const [rejectDialogOpen, setRejectDialogOpen] = useState(false);
  const [selectedShift, setSelectedShift] = useState<Shift | null>(null);

  // Hooks
  const { data: pendingShifts = [], isLoading: isPendingLoading } = usePendingShifts();
  const { data: allShifts = [], isLoading: isAllLoading } = useManagedShifts();
  const approveShiftMutation = useApproveShift();
  const rejectShiftMutation = useRejectShift();

  const isLoading = isPendingLoading || isAllLoading;

  const handleApprove = async (id: string) => {
    await approveShiftMutation.mutateAsync(id);
    setApproveDialogOpen(false);
    setSelectedShift(null);
  };

  const handleReject = async (id: string, rejectionReason: string) => {
    await rejectShiftMutation.mutateAsync({ id, data: { rejectionReason } });
    setRejectDialogOpen(false);
    setSelectedShift(null);
  };

  const handleApproveClick = (shift: Shift) => {
    setSelectedShift(shift);
    setApproveDialogOpen(true);
  };

  const handleRejectClick = (shift: Shift) => {
    setSelectedShift(shift);
    setRejectDialogOpen(true);
  };

  const value: ShiftManagementContextValue = {
    // State
    pendingShifts,
    allShifts,
    isLoading,
    
    // Dialog states
    approveDialogOpen,
    rejectDialogOpen,
    selectedShift,
    
    // Actions
    setApproveDialogOpen,
    setRejectDialogOpen,
    handleApprove,
    handleReject,
    handleApproveClick,
    handleRejectClick,
  };

  return (
    <ShiftManagementContext.Provider value={value}>
      {children}
    </ShiftManagementContext.Provider>
  );
};
