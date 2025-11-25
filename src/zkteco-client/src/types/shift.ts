export enum ShiftStatus {
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3,
    ApprovedLeave = 5,
}

export interface Shift {
    id: string;
    applicationUserId: string;
    employeeName: string;
    startTime: string;
    endTime: string;
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
    description?: string;
}

export interface UpdateShiftRequest {
    startTime: string;
    endTime: string;
    description?: string;
}

export interface RejectShiftRequest {
    rejectionReason: string;
}

export interface ShiftResponse {
    isSuccess: boolean;
    data?: Shift;
    errors: string[];
    message: string;
}

export interface ShiftListResponse {
    isSuccess: boolean;
    data?: Shift[];
    errors: string[];
    message: string;
}

// Shift Template types
export interface ShiftTemplate {
    id: string;
    createdByUserId: string;
    name: string;
    startTime: string;
    endTime: string;
    totalHours: number;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
}

export interface CreateShiftTemplateRequest {
    name: string;
    startTime: string;
    endTime: string;
}

export interface UpdateShiftTemplateRequest {
    name: string;
    startTime: string;
    endTime: string;
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
