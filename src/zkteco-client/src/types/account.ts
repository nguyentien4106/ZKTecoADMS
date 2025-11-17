export interface EmployeeAccount {
    email: string
    firstName: string
    lastName: string
    phoneNumber?: string
    employeeDeviceId: string
}

export interface CreateEmployeeAccountRequest {
  email: string
  password: string
  employeeDeviceId: string
  firstName: string
  lastName: string
  phoneNumber?: string
}

export interface UpdateEmployeeAccountRequest {
  email: string
  firstName: string
  lastName: string
  phoneNumber?: string
  password?: string
}