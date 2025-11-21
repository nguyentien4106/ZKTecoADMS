export enum LeaveType {
  SICK = 0,
  VACATION = 1,
  PERSONAL = 2,
  OTHER = 3
}

export enum LeaveStatus {
  PENDING = 0,
  APPROVED = 1,
  REJECTED = 2,
  CANCELLED = 3
}

// Helper function to get leave type display name
export const getLeaveTypeLabel = (type: LeaveType): string => {
  switch (type) {
    case LeaveType.SICK:
      return 'Sick';
    case LeaveType.VACATION:
      return 'Vacation';
    case LeaveType.PERSONAL:
      return 'Personal';
    case LeaveType.OTHER:
      return 'Other';
    default:
      return 'Unknown';
  }
};

// Helper function to get leave status display name
export const getLeaveStatusLabel = (status: LeaveStatus): string => {
  switch (status) {
    case LeaveStatus.PENDING:
      return 'Pending';
    case LeaveStatus.APPROVED:
      return 'Approved';
    case LeaveStatus.REJECTED:
      return 'Rejected';
    case LeaveStatus.CANCELLED:
      return 'Cancelled';
    default:
      return 'Unknown';
  }
};

export interface LeaveRequest {
  id: string;
  employeeId: string;
  employeeName: string;
  type: LeaveType;
  startDate: string; // ISO Date string
  endDate: string; // ISO Date string
  isFullDay: boolean;
  reason: string;
  status: LeaveStatus;
  rejectionReason?: string;
  createdAt: string;
}

export interface CreateLeaveRequest {
  type: LeaveType;
  startDate: string;
  endDate: string;
  isFullDay: boolean;
  reason: string;
}

export interface RejectLeaveRequest {
  reason: string;
}
