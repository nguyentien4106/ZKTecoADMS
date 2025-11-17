// ==========================================
// src/services/accountService.ts
// ==========================================
import { apiService } from './api'

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

export const accountService = {
  createUserAccount: async (employeeAccount: CreateEmployeeAccountRequest) => {
    return await apiService.post<boolean>(
      '/api/Accounts',
      employeeAccount
    )
  },

  updateUserAccount: async (
    employeeDeviceId: string,
    employeeAccount: UpdateEmployeeAccountRequest
  ) => {
    return await apiService.put<boolean>(
      `/api/Accounts/${employeeDeviceId}`,
      employeeAccount
    )
  },
}
