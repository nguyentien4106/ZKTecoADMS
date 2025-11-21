// ==========================================
// src/contexts/LeaveContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode } from 'react';
import { LeaveRequest, CreateLeaveRequest } from '@/types/leave';
import { 
  usePendingLeaves, 
  useAllLeaves, 
  useApproveLeave, 
  useRejectLeave,
  useCreateLeave,
  useCancelLeave,
} from '@/hooks/useLeaves';

interface LeaveContextValue {
  // State
  pendingLeaves: LeaveRequest[];
  allLeaves: LeaveRequest[];
  isLoading: boolean;
  
  // Dialog states
  approveDialogOpen: boolean;
  rejectDialogOpen: boolean;
  cancelDialogOpen: boolean;
  createDialogOpen: boolean;
  selectedLeave: LeaveRequest | null;
  rejectionReason: string;
  
  // Actions
  setApproveDialogOpen: (open: boolean) => void;
  setRejectDialogOpen: (open: boolean) => void;
  setCancelDialogOpen: (open: boolean) => void;
  setCreateDialogOpen: (open: boolean) => void;
  setRejectionReason: (reason: string) => void;
  handleApprove: (id: string) => Promise<void>;
  handleReject: (id: string, rejectionReason: string) => Promise<void>;
  handleCancel: (id: string) => Promise<void>;
  handleCreate: (data: CreateLeaveRequest) => Promise<void>;
  handleApproveClick: (leave: LeaveRequest) => void;
  handleRejectClick: (leave: LeaveRequest) => void;
  handleCancelClick: (leave: LeaveRequest) => void;
}

const LeaveContext = createContext<LeaveContextValue | undefined>(undefined);

export const useLeaveContext = () => {
  const context = useContext(LeaveContext);
  if (!context) {
    throw new Error('useLeaveContext must be used within LeaveProvider');
  }
  return context;
};

interface LeaveProviderProps {
  children: ReactNode;
}

export const LeaveProvider = ({ children }: LeaveProviderProps) => {
  // Dialog states
  const [approveDialogOpen, setApproveDialogOpen] = useState(false);
  const [rejectDialogOpen, setRejectDialogOpen] = useState(false);
  const [cancelDialogOpen, setCancelDialogOpen] = useState(false);
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [selectedLeave, setSelectedLeave] = useState<LeaveRequest | null>(null);
  const [rejectionReason, setRejectionReason] = useState('');

  // Hooks
  const { data: pendingLeaves = [], isLoading: isPendingLoading } = usePendingLeaves();
  const { data: allLeaves = [], isLoading: isAllLoading } = useAllLeaves();

  const approveLeaveMutation = useApproveLeave();
  const rejectLeaveMutation = useRejectLeave();
  const cancelLeaveMutation = useCancelLeave();
  const createLeaveMutation = useCreateLeave();
  
  const isLoading = isPendingLoading || isAllLoading;

  const handleApprove = async (id: string) => {
    await approveLeaveMutation.mutateAsync(id);
    setApproveDialogOpen(false);
    setSelectedLeave(null);
  };

  const handleReject = async (id: string, rejectionReason: string) => {
    await rejectLeaveMutation.mutateAsync({ id, data: { reason: rejectionReason } });
    setRejectDialogOpen(false);
    setSelectedLeave(null);
    setRejectionReason('');
  };

  const handleCancel = async (id: string) => {
    await cancelLeaveMutation.mutateAsync(id);
    setCancelDialogOpen(false);
    setSelectedLeave(null);
  };

  const handleCreate = async (data: CreateLeaveRequest) => {
    await createLeaveMutation.mutateAsync(data);
    setCreateDialogOpen(false);
  };

  const handleApproveClick = (leave: LeaveRequest) => {
    setSelectedLeave(leave);
    setApproveDialogOpen(true);
  };

  const handleRejectClick = (leave: LeaveRequest) => {
    setSelectedLeave(leave);
    setRejectDialogOpen(true);
  };

  const handleCancelClick = (leave: LeaveRequest) => {
    setSelectedLeave(leave);
    setCancelDialogOpen(true);
  };

  const value: LeaveContextValue = {
    // State
    pendingLeaves,
    allLeaves,
    isLoading,
    
    // Dialog states
    approveDialogOpen,
    rejectDialogOpen,
    cancelDialogOpen,
    createDialogOpen,
    selectedLeave,
    rejectionReason,
    
    // Actions
    setApproveDialogOpen,
    setRejectDialogOpen,
    setCancelDialogOpen,
    setCreateDialogOpen,
    setRejectionReason,
    handleApprove,
    handleReject,
    handleCancel,
    handleCreate,
    handleApproveClick,
    handleRejectClick,
    handleCancelClick,
  };

  return (
    <LeaveContext.Provider value={value}>
      {children}
    </LeaveContext.Provider>
  );
};
