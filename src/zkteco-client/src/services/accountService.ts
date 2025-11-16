// ==========================================
// src/services/accountService.ts
// ==========================================
import { apiService } from './api'

export interface CreateUserAccountRequest {
  email: string
  password: string
  userDeviceId: string
  firstName: string
  lastName: string
  phoneNumber?: string
  department?: string
}

export const accountService = {
  createUserAccount: async (
    userDeviceId: string,
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    phoneNumber?: string,
    department?: string
  ) => {
    return await apiService.post<boolean>(
      '/api/Accounts',
      {
        email,
        password,
        userDeviceId,
        firstName,
        lastName,
        phoneNumber,
        department,
      }
    )

  },
}
