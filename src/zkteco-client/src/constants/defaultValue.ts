import { CreateDeviceRequest, CreateUserRequest } from "@/types";

export const defaultNewUser: CreateUserRequest = {
    pin: '',
    fullName: '',
    cardNumber: '',
    password: '',
    email: '',
    phoneNumber: '',
    department: '',
    position: '',
    groupId: 1,
    privilege: 0,
    verifyMode: 0,
}

export const defaultNewDevice: CreateDeviceRequest = {
    serialNumber: '',
    deviceName: '',
    model: '',
    ipAddress: '',
    port: 4370,
    location: '',
    applicationUserId: ''
}