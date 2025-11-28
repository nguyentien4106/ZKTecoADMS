export enum ShiftStatus {
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3,
    ApprovedLeave = 5,
}

export interface Shift {
    id: string;
    employeeUserId: string;
    employeeName: string;
    startTime: string;
    endTime: string;
    maximumAllowedLateMinutes: number;
    maximumAllowedEarlyLeaveMinutes: number;
    description?: string;
    status: ShiftStatus;
    approvedByUserId?: string;
    approvedByUserName?: string;
    approvedAt?: string;
    rejectionReason?: string;
    createdAt: string;
    updatedAt?: string;
    totalHours: number;
}

export interface CreateShiftRequest {
    employeeUserId?: string | null;
    startTime: string;
    endTime: string;
    maximumAllowedLateMinutes?: number;
    maximumAllowedEarlyLeaveMinutes?: number;
    description?: string;
}



export interface CreatShiftDialog {
    employeeUserId: string | null;
    startTime: Date;
    endTime: Date;
    maximumAllowedLateMinutes: number;
    maximumAllowedEarlyLeaveMinutes: number;
    description: string;
}

export interface UpdateShiftRequest {
    startTime: string;
    endTime: string;
    maximumAllowedLateMinutes?: number;
    maximumAllowedEarlyLeaveMinutes?: number;
    description?: string;
}

export interface RejectShiftRequest {
    rejectionReason: string;
}

// Shift Template types
export interface ShiftTemplate {
    id: string;
    createdByUserId: string;
    name: string;
    startTime: string;
    endTime: string;
    maximumAllowedLateMinutes: number;
    maximumAllowedEarlyLeaveMinutes: number;
    totalHours: number;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
}

export interface UpdateShiftTemplateRequest {
    startTime: string;
    endTime: string;
    maximumAllowedLateMinutes?: number;
    maximumAllowedEarlyLeaveMinutes?: number;
    name: string,
    isActive: boolean;
}

export interface CreateShiftTemplateRequest {
    name: string;
    startTime: string;
    endTime: string;
    maximumAllowedLateMinutes?: number;
    maximumAllowedEarlyLeaveMinutes?: number;
}

export interface UpdateShiftTemplateRequest {
    name: string;
    startTime: string;
    endTime: string;
    maximumAllowedLateMinutes?: number;
    maximumAllowedEarlyLeaveMinutes?: number;
    isActive: boolean;
}

export interface ShiftTemplateResponse {
    isSuccess: boolean;
    data?: ShiftTemplate;
    errors: string[];
    message: string;
}

export interface ShiftTemplateListResponse {
    isSuccess: boolean;
    data?: ShiftTemplate[];
    errors: string[];
    message: string;
}
