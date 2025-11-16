import { CreateDeviceRequest, PaginationRequest } from "@/types";
import { AttendancesFilterParams } from "@/types/attendance";
import { CreateUserRequest } from "@/types/user";
import { subMonths } from "date-fns";

export const defaultNewUser: CreateUserRequest = {
    pin: '',
    name: '',
    cardNumber: '',
    password: '',
    email: '',
    phoneNumber: '',
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
    fromDate: threeMonthsAgo,
    toDate: today,
    deviceIds: []
}

export const defaultAttendancePaginationRequest: PaginationRequest = {
    pageNumber: 1,
    pageSize: 50,
    sortBy: 'AttendanceTime',
    sortOrder: 'desc'
}