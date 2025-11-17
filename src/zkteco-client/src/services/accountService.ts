// ==========================================
// src/services/accountService.ts
// ==========================================
import { CreateEmployeeAccountRequest, EmployeeAccount, UpdateEmployeeAccountRequest } from '@/types/account'
import { apiService } from './api'

export const accountService = {
  createUserAccount: async (employeeAccount: CreateEmployeeAccountRequest) => {
    return await apiService.post<EmployeeAccount>(
      '/api/Accounts',
      employeeAccount
    )
  },

  updateUserAccount: async (
    employeeDeviceId: string,
    employeeAccount: UpdateEmployeeAccountRequest
  ) => {
    return await apiService.put<EmployeeAccount>(
      `/api/Accounts/${employeeDeviceId}`,
      employeeAccount
    )
  },
}
