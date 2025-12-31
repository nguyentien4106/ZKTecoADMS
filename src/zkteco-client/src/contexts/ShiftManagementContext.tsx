// ==========================================
// src/contexts/ShiftManagementContext.tsx
// ==========================================
import { createContext, useContext, useState, ReactNode, Dispatch, useEffect, useMemo } from 'react';
import { CreateShiftRequest, Shift, ShiftTemplate, UpdateShiftTemplateRequest, CreateShiftTemplateRequest, ShiftManagementFilter } from '@/types/shift';
import { 
  usePendingShifts, 
  useManagedShifts, 
  useApproveShift,
  useApproveShifts, 
  useRejectShift,
  useRejectShifts,
  useCreateShift,
  useAssignShift,
} from '@/hooks/useShifts';
import {
  useShiftTemplates,
  useCreateShiftTemplate,
  useUpdateShiftTemplate,
  useDeleteShiftTemplate
} from '@/hooks/useShiftTemplate';
import { PaginatedResponse, PaginationRequest } from '@/types';
import { defaultShiftPaginationRequest } from '@/constants/defaultValue';
import { Employee } from '@/types/employee';
import { useEmployees } from '@/hooks/useEmployee';

interface ShiftManagementContextValue {
  // State
  pendingPaginationRequest: PaginationRequest;
  allPaginationRequest: PaginationRequest;
  pendingPaginatedShifts: PaginatedResponse<Shift> | undefined;
  allPaginatedShifts: PaginatedResponse<Shift> | undefined;
  templates: ShiftTemplate[];
  isLoading: boolean;
  employees: Employee[];
  filters: ShiftManagementFilter;

  setPendingPaginationRequest: Dispatch<React.SetStateAction<PaginationRequest>>;
  setAllPaginationRequest: Dispatch<React.SetStateAction<PaginationRequest>>;
  setFilters: Dispatch<React.SetStateAction<ShiftManagementFilter>>;
  
  // Dialog states
  approveDialogOpen: boolean;
  rejectDialogOpen: boolean;
  selectedShift: Shift | null;
  createTemplateDialogOpen: boolean;
  updateTemplateDialogOpen: boolean;
  selectedTemplate: ShiftTemplate | null;
  assignShiftDialogOpen: boolean;
  editShiftDialogOpen: boolean;
  
  // Actions
  setApproveDialogOpen: (open: boolean) => void;
  setRejectDialogOpen: (open: boolean) => void;
  handleApprove: (id: string) => Promise<void>;
  handleReject: (id: string, rejectionReason: string) => Promise<void>;
  handleApproveClick: (shift: Shift) => void;
  handleRejectClick: (shift: Shift) => void;
  handleApproveSelected: (ids: string[]) => Promise<void>;
  handleRejectSelected: (ids: string[], rejectionReason: string) => Promise<void>;
  
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
  
  // Edit shift actions
  setEditShiftDialogOpen: (open: boolean) => void;
  handleEditShiftClick: (shift: Shift) => void;

  handleAssignShift: (data: CreateShiftRequest & { employeeId: string }) => Promise<void>;
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

const defaultFilter: ShiftManagementFilter = {
    employeeIds: [],
    month: new Date().getMonth() + 1,
    year: new Date().getFullYear(),
};

export const ShiftManagementProvider = ({ children }: ShiftManagementProviderProps) => {
  // Dialog states
  const [approveDialogOpen, setApproveDialogOpen] = useState(false);
  const [rejectDialogOpen, setRejectDialogOpen] = useState(false);
  const [selectedShift, setSelectedShift] = useState<Shift | null>(null);
  const [createTemplateDialogOpen, setCreateTemplateDialogOpen] = useState(false);
  const [updateTemplateDialogOpen, setUpdateTemplateDialogOpen] = useState(false);
  const [selectedTemplate, setSelectedTemplate] = useState<ShiftTemplate | null>(null);
  const [assignShiftDialogOpen, setAssignShiftDialogOpen] = useState(false);
  const [editShiftDialogOpen, setEditShiftDialogOpen] = useState(false);
  const [pendingPaginationRequest, setPendingPaginationRequest] = useState(defaultShiftPaginationRequest);
  const [allPaginationRequest, setAllPaginationRequest] = useState(defaultShiftPaginationRequest);
  const [filters, setFilters] = useState<ShiftManagementFilter>(defaultFilter);

  // Hooks
  const { data: pendingPaginatedShifts, isLoading: isPendingLoading } = usePendingShifts(pendingPaginationRequest);
  const { data: templates = [], isLoading: isTemplatesLoading } = useShiftTemplates();
  const { data: employees = [], isLoading: isEmployeesLoading } = useEmployees({ employmentType: "0" }); // Hourly

  // Memoize employee IDs to prevent unnecessary re-renders
  const employeeIds = useMemo(() => employees.map(emp => emp.id), [employees]);

  // Update filters when employee IDs change
  useEffect(() => {
    setFilters((prev) => {
      // Only update if employee IDs have actually changed
      const prevIds = prev.employeeIds.sort().join(',');
      const newIds = employeeIds.sort().join(',');
      if (prevIds !== newIds) {
        return {
          ...prev,
          employeeIds: employeeIds,
        };
      }
      return prev;
    });
  }, [employeeIds]);

  const { data: allPaginatedShifts, isLoading: isAllLoading } = useManagedShifts(allPaginationRequest, filters)
  
  const approveShiftMutation = useApproveShift();
  const rejectShiftMutation = useRejectShift();
  const approveShiftsMutation = useApproveShifts();
  const rejectShiftsMutation = useRejectShifts();
  const createTemplateMutation = useCreateShiftTemplate();
  const updateTemplateMutation = useUpdateShiftTemplate();
  const deleteTemplateMutation = useDeleteShiftTemplate();
  const createShiftMutation = useCreateShift();
  const assignShiftMutation = useAssignShift();
  
  const isLoading = isPendingLoading || isAllLoading || isTemplatesLoading || isEmployeesLoading;

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

  const handleApproveSelected = async (ids: string[]) => {
    await approveShiftsMutation.mutateAsync(ids);
  };

  const handleRejectSelected = async (ids: string[], rejectionReason: string) => {
    await rejectShiftsMutation.mutateAsync({ ids, rejectionReason });
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

  const handleAssignShift = async (data: CreateShiftRequest & { employeeId: string }) => {
    await assignShiftMutation.mutateAsync(data);
    setAssignShiftDialogOpen(false);
  }

  const handleEditShiftClick = (shift: Shift) => {
    setSelectedShift(shift);
    setEditShiftDialogOpen(true);
  };

  const value: ShiftManagementContextValue = {
    // State
    pendingPaginationRequest,
    allPaginationRequest,
    pendingPaginatedShifts,
    allPaginatedShifts,
    templates,
    isLoading,
    employees,
    filters,

    setAllPaginationRequest,
    setPendingPaginationRequest,
    setFilters,
    
    // Dialog states
    approveDialogOpen,
    rejectDialogOpen,
    selectedShift,
    createTemplateDialogOpen,
    updateTemplateDialogOpen,
    selectedTemplate,
    assignShiftDialogOpen,
    editShiftDialogOpen,
    
    // Actions
    setApproveDialogOpen,
    setRejectDialogOpen,
    handleApprove,
    handleReject,
    handleApproveClick,
    handleRejectClick,
    handleApproveSelected,
    handleRejectSelected,
    
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
    
    // Edit shift actions
    setEditShiftDialogOpen,
    handleEditShiftClick,

    handleAssignShift
  };

  return (
    <ShiftManagementContext.Provider value={value}>
      {children}
    </ShiftManagementContext.Provider>
  );
};
