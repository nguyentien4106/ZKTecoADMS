export enum ShiftStatus {
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3
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
