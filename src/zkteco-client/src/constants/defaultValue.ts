import { LeaveDialogState, LeaveStatus, LeaveType } from '@/types/leave';
import { CreateDeviceRequest, PaginationRequest } from "@/types";
import { AttendancesFilterParams } from "@/types/attendance";
import { CreateEmployeeRequest } from "@/types/employee";
import { CreateShiftRequest, CreateShiftTemplateRequest } from "@/types/shift";
import { format, subMonths } from "date-fns";
import { DateTimeFormat } from ".";

export const defaultNewEmployee: CreateEmployeeRequest = {
    pin: '',
    name: '',
    cardNumber: '',
    password: '',
    department: '',
    privilege: 0,
}

export const defaultNewDevice: CreateDeviceRequest = {
    serialNumber: '',
    deviceName: '',
    location: '',
    description: '',
}

const today = new Date()
const threeMonthsAgo = subMonths(today, 3)

export const defaultAttendanceFilter: AttendancesFilterParams = {
    fromDate: format(threeMonthsAgo, DateTimeFormat),
    toDate: format(today, DateTimeFormat),
    deviceIds: []
}

export const defaultAttendancePaginationRequest: PaginationRequest = {
    pageNumber: 1,
    pageSize: 50,
    sortBy: 'AttendanceTime',
    sortOrder: 'desc'
}

export const defaultNewShift: CreateShiftRequest = {
    startTime: '',
    endTime: '',
    description: ''
}

export const defaultNewShiftTemplate: CreateShiftTemplateRequest = {
    name: '',
    startTime: '09:00:00',
    endTime: '17:00:00',
}

const now = new Date();
const tomorrow = new Date(now);
tomorrow.setDate(tomorrow.getDate() + 1);

export const defaultNewShiftWithEmployeeUserId = {
    employeeUserId: null,
    startTime: tomorrow,
    endTime: tomorrow,
    description: ''
}

export const defaultTemplateShift = {
    templateId: '',
    date: new Date(),
    description: '',
}

export const defaultLeaveDialogState: LeaveDialogState = {
    employeeUserId: null,
    shiftId: '',
    type: LeaveType.PERSONAL,
    isHalfShift: false,
    halfShiftType: '',
    startDate: undefined,
    endDate: undefined,
    reason: '',
    status: LeaveStatus.APPROVED
};