// ==========================================
// src/contexts/LeaveContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode } from 'react';
import { LeaveRequest, CreateLeaveRequest, UpdateLeaveRequest, LeaveDialogState } from '@/types/leave';
import { 
  usePendingLeaves, 
  useAllLeaves, 
  useApproveLeave, 
  useRejectLeave,
  useCreateLeave,
  useUpdateLeave,
  useCancelLeave,
} from '@/hooks/useLeaves';
import { format } from 'date-fns';
import { DateTimeFormat } from '@/constants';

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
  editDialogOpen: boolean;
  selectedLeave: LeaveRequest | null;
  rejectionReason: string;
  
  // Actions
  setApproveDialogOpen: (open: boolean) => void;
  setRejectDialogOpen: (open: boolean) => void;
  setCancelDialogOpen: (open: boolean) => void;
  setCreateDialogOpen: (open: boolean) => void;
  setEditDialogOpen: (open: boolean) => void;
  setRejectionReason: (reason: string) => void;
  handleApprove: (id: string) => Promise<void>;
  handleReject: (id: string, rejectionReason: string) => Promise<void>;
  handleCancel: (id: string) => Promise<void>;
  handleCreate: (data: CreateLeaveRequest) => Promise<void>;
  handleUpdate: (id: string, data: UpdateLeaveRequest) => Promise<void>;
  handleApproveClick: (leave: LeaveRequest) => void;
  handleRejectClick: (leave: LeaveRequest) => void;
  handleCancelClick: (leave: LeaveRequest) => void;
  handleEditClick: (leave: LeaveRequest) => void;

  handleAddOrUpdate: (data: CreateLeaveRequest | UpdateLeaveRequest | LeaveDialogState, id?: string) => Promise<void>;

  dialogMode: 'create' | 'edit' | null;
  setDialogMode: (mode: 'create' | 'edit' | null) => void;
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
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [selectedLeave, setSelectedLeave] = useState<LeaveRequest | null>(null);
  const [rejectionReason, setRejectionReason] = useState('');

  const [dialogMode, setDialogMode] = useState<'create' | 'edit' | null>(null);

  // Hooks
  const { data: pendingLeaves = [], isLoading: isPendingLoading } = usePendingLeaves();
  const { data: allLeaves = [], isLoading: isAllLoading } = useAllLeaves();

  const approveLeaveMutation = useApproveLeave();
  const rejectLeaveMutation = useRejectLeave();
  const cancelLeaveMutation = useCancelLeave();
  const createLeaveMutation = useCreateLeave();
  const updateLeaveMutation = useUpdateLeave();
  
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

  const handleUpdate = async (id: string, data: UpdateLeaveRequest) => {
    await updateLeaveMutation.mutateAsync({ id, data });
    setEditDialogOpen(false);
    setSelectedLeave(null);
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

  const handleEditClick = (leave: LeaveRequest) => {
    setDialogMode('edit');
    setSelectedLeave(leave);
    setEditDialogOpen(true);
  };

  const handleAddOrUpdate = async (data: CreateLeaveRequest | UpdateLeaveRequest | LeaveDialogState, id?: string) => {
    data.startDate = format(data.startDate as Date, DateTimeFormat);
    data.endDate = format(data.endDate as Date, DateTimeFormat);
    
    if( dialogMode === 'create') {
      await handleCreate(data as CreateLeaveRequest);
    } else if (dialogMode === 'edit' && id) {
      await handleUpdate(id, data as UpdateLeaveRequest);
    }
  }

  const value: LeaveContextValue = {
    // State
    pendingLeaves,
    allLeaves,
    isLoading,
    dialogMode,
    setDialogMode,

    // Dialog states
    approveDialogOpen,
    rejectDialogOpen,
    cancelDialogOpen,
    createDialogOpen,
    editDialogOpen,
    selectedLeave,
    rejectionReason,
    
    // Actions
    setApproveDialogOpen,
    setRejectDialogOpen,
    setCancelDialogOpen,
    setCreateDialogOpen,
    setEditDialogOpen,
    setRejectionReason,
    handleApprove,
    handleReject,
    handleCancel,
    handleCreate,
    handleUpdate,
    handleApproveClick,
    handleRejectClick,
    handleCancelClick,
    handleEditClick,
    handleAddOrUpdate
  };

  return (
    <LeaveContext.Provider value={value}>
      {children}
    </LeaveContext.Provider>
  );
};
