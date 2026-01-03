// ==========================================
// src/constants/path.ts
// Application route paths
// ==========================================

export const PATHS = {
  // Public routes
  LOGIN: '/login',
  FORGOT_PASSWORD: '/forgot-password',
  RESET_PASSWORD: '/reset-password',
  DEMO_DASHBOARD: '/demo-dashboard',

  // Root
  ROOT: '/',

  // Common authenticated routes
  DASHBOARD: '/dashboard',
  SETTINGS: '/settings',
  PROFILE: '/profile',

  // Attendance
  ATTENDANCE: '/attendance',
  ATTENDANCE_SUMMARY: '/attendance-summary',

  // Shifts
  MY_SHIFTS: '/my-shifts',
  SHIFTS: '/shifts',
  PENDING_SHIFTS: '/pending-shifts',
  SHIFT_TEMPLATES: '/shift-templates',
  SHIFT_EXCHANGE: '/shift-exchange',

  // Leaves
  LEAVES: '/leaves',
  PENDING_LEAVES: '/pending-leaves',
  MY_LEAVES: '/my-leaves',
  ALL_LEAVES: '/all-leaves',

  // Payslips
  PAYSLIPS: '/payslips',

  // Devices
  DEVICES: '/devices',
  DEVICE_COMMANDS: '/device-commands',
  DEVICE_USERS: '/device-users',

  // Employees
  EMPLOYEES: '/employees',
  EMPLOYEE_BENEFITS: '/employee-benefits',

  // Benefits
  BENEFITS: '/benefits',

  // Reports
  REPORTS: '/reports',
} as const

// Type for all valid paths
export type AppPath = (typeof PATHS)[keyof typeof PATHS]
