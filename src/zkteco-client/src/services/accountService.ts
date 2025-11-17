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
}

export interface UpdateUserAccountRequest {
  email: string
  firstName: string
  lastName: string
  phoneNumber?: string
  password?: string
}

export const accountService = {
  createUserAccount: async (
    userDeviceId: string,
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    phoneNumber?: string
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
      }
    )
  },

  updateUserAccount: async (
    userDeviceId: string,
    firstName: string,
    lastName: string,
    email: string,
    phoneNumber?: string,
    password?: string
  ) => {
    return await apiService.put<boolean>(
      `/api/Accounts/${userDeviceId}`,
      {
        email,
        firstName,
        lastName,
        phoneNumber,
        password,
      }
    )
  },
}
