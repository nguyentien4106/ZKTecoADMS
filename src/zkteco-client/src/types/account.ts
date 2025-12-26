export interface Account {
    id: string
    email: string
    firstName: string
    lastName: string
    phoneNumber?: string
    employeeDeviceId: string
    fullName: string
    userName?: string
}

export interface CreateEmployeeAccountRequest {
  email: string
  password: string
  firstName: string
  lastName: string
  phoneNumber?: string
  userName: string
  employeeId: string
}

export interface UpdateEmployeeAccountRequest {
  email: string
  firstName: string
  lastName: string
  phoneNumber?: string
  password?: string
  userName?: string
}