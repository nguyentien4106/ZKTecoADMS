// ==========================================
// src/constants/roles.ts
// ==========================================

import { PATHS } from "./path"

export enum UserRole {
  ADMIN = 'Admin',
  MANAGER = 'Manager',
  EMPLOYEE = 'Employee'
}

export const ROLE_HIERARCHY = {
  [UserRole.ADMIN]: 3,
  [UserRole.MANAGER]: 2,
  [UserRole.EMPLOYEE]: 1
}


// Define which routes are accessible by each role
export const ROLE_PERMISSIONS = {
  [UserRole.ADMIN]: [
    PATHS.DASHBOARD,
    PATHS.DEVICES,
    PATHS.DEVICE_COMMANDS,
    PATHS.EMPLOYEES,
    PATHS.ATTENDANCE,
    PATHS.REPORTS,
    PATHS.SETTINGS,
    PATHS.SHIFTS,
    PATHS.SHIFT_TEMPLATES,
    PATHS.LEAVES,
    PATHS.BENEFITS,
    PATHS.PAYSLIPS
  ],
  [UserRole.MANAGER]: [
    PATHS.DASHBOARD,

    PATHS.EMPLOYEES,
    PATHS.EMPLOYEE_BENEFITS,
    
    PATHS.DEVICES,
    PATHS.DEVICE_COMMANDS,
    PATHS.DEVICE_USERS,

    PATHS.ATTENDANCE,
    PATHS.ATTENDANCE_SUMMARY,

    PATHS.SHIFTS,
    PATHS.SHIFT_TEMPLATES,
    PATHS.PENDING_SHIFTS,

    PATHS.REPORTS,
    PATHS.SETTINGS,
    PATHS.LEAVES,
    PATHS.BENEFITS,
    PATHS.PAYSLIPS
  ],
  [UserRole.EMPLOYEE]: [
    PATHS.DASHBOARD,
    PATHS.ATTENDANCE,
    PATHS.SETTINGS,
    PATHS.EMPLOYEES,
    PATHS.MY_SHIFTS,
    PATHS.LEAVES,
    PATHS.PAYSLIPS
  ]
}
