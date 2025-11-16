// ==========================================
// src/constants/roles.ts
// ==========================================

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
    '/dashboard',
    '/devices',
    '/device-commands',
    '/users',
    '/attendance',
    '/reports',
    '/settings'
  ],
  [UserRole.MANAGER]: [
    '/dashboard',
    '/devices',
    '/device-commands',
    '/users',
    '/attendance',
    '/reports',
    '/settings'
  ],
  [UserRole.EMPLOYEE]: [
    '/dashboard',
    '/attendance',
    '/settings',
    '/users'
  ]
}
