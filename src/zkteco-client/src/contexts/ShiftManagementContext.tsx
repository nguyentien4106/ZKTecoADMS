// ==========================================
// src/contexts/ShiftManagementContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode } from 'react';
import { CreateShiftRequest, Shift, ShiftTemplate, UpdateShiftTemplateRequest, CreateShiftTemplateRequest } from '@/types/shift';
import { 
  usePendingShifts, 
  useManagedShifts, 
  useApproveShift, 
  useRejectShift,
  useCreateShift,
} from '@/hooks/useShifts';
import {
  useShiftTemplates,
  useCreateShiftTemplate,
  useUpdateShiftTemplate,
  useDeleteShiftTemplate
} from '@/hooks/useShiftTemplate';

interface ShiftManagementContextValue {
  // State
  pendingShifts: Shift[];
  allShifts: Shift[];
  templates: ShiftTemplate[];
  isLoading: boolean;
  
  // Dialog states
  approveDialogOpen: boolean;
  rejectDialogOpen: boolean;
  selectedShift: Shift | null;
  createTemplateDialogOpen: boolean;
  updateTemplateDialogOpen: boolean;
  selectedTemplate: ShiftTemplate | null;
  assignShiftDialogOpen: boolean;
  
  // Actions
  setApproveDialogOpen: (open: boolean) => void;
  setRejectDialogOpen: (open: boolean) => void;
  handleApprove: (id: string) => Promise<void>;
  handleReject: (id: string, rejectionReason: string) => Promise<void>;
  handleApproveClick: (shift: Shift) => void;
  handleRejectClick: (shift: Shift) => void;
  
  // Template actions
  setCreateTemplateDialogOpen: (open: boolean) => void;
  setUpdateTemplateDialogOpen: (open: boolean) => void;
  handleCreateTemplate: (data: CreateShiftTemplateRequest) => Promise<void>;
  handleUpdateTemplate: (id: string, data: UpdateShiftTemplateRequest) => Promise<void>;
  handleDeleteTemplate: (id: string) => Promise<void>;
  handleEditTemplateClick: (template: ShiftTemplate) => void;
  
  // Assign shift actions
  setAssignShiftDialogOpen: (open: boolean) => void;
  handleCreateShift: (data: CreateShiftRequest) => Promise<void>;
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
  const [createTemplateDialogOpen, setCreateTemplateDialogOpen] = useState(false);
  const [updateTemplateDialogOpen, setUpdateTemplateDialogOpen] = useState(false);
  const [selectedTemplate, setSelectedTemplate] = useState<ShiftTemplate | null>(null);
  const [assignShiftDialogOpen, setAssignShiftDialogOpen] = useState(false);

  // Hooks
  const { data: pendingShifts = [], isLoading: isPendingLoading } = usePendingShifts();
  const { data: allShifts = [], isLoading: isAllLoading } = useManagedShifts();
  const { data: templates = [], isLoading: isTemplatesLoading } = useShiftTemplates();

  const approveShiftMutation = useApproveShift();
  const rejectShiftMutation = useRejectShift();
  const createTemplateMutation = useCreateShiftTemplate();
  const updateTemplateMutation = useUpdateShiftTemplate();
  const deleteTemplateMutation = useDeleteShiftTemplate();
  const createShiftMutation = useCreateShift();
  
  const isLoading = isPendingLoading || isAllLoading || isTemplatesLoading;

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

  const handleCreateTemplate = async (data: { name: string; startTime: string; endTime: string }) => {
    await createTemplateMutation.mutateAsync(data);
    setCreateTemplateDialogOpen(false);
  };

  const handleUpdateTemplate = async (id: string, data: { name: string; startTime: string; endTime: string; isActive: boolean }) => {
    await updateTemplateMutation.mutateAsync({ id, data });
    setUpdateTemplateDialogOpen(false);
    setSelectedTemplate(null);
  };

  const handleDeleteTemplate = async (id: string) => {
    await deleteTemplateMutation.mutateAsync(id);
  };

  const handleEditTemplateClick = (template: ShiftTemplate) => {
    setSelectedTemplate(template);
    setUpdateTemplateDialogOpen(true);
  };

  const handleCreateShift = async (data: CreateShiftRequest) => {
    await createShiftMutation.mutateAsync(data);
    setAssignShiftDialogOpen(false);
  };

  const value: ShiftManagementContextValue = {
    // State
    pendingShifts,
    allShifts,
    templates,
    isLoading,
    
    // Dialog states
    approveDialogOpen,
    rejectDialogOpen,
    selectedShift,
    createTemplateDialogOpen,
    updateTemplateDialogOpen,
    selectedTemplate,
    assignShiftDialogOpen,
    
    // Actions
    setApproveDialogOpen,
    setRejectDialogOpen,
    handleApprove,
    handleReject,
    handleApproveClick,
    handleRejectClick,
    
    // Template actions
    setCreateTemplateDialogOpen,
    setUpdateTemplateDialogOpen,
    handleCreateTemplate,
    handleUpdateTemplate,
    handleDeleteTemplate,
    handleEditTemplateClick,
    
    // Assign shift actions
    setAssignShiftDialogOpen,
    handleCreateShift,
  };

  return (
    <ShiftManagementContext.Provider value={value}>
      {children}
    </ShiftManagementContext.Provider>
  );
};
