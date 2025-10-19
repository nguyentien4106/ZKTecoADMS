// ==========================================
// src/types/index.ts
// ==========================================
export interface Device {
  id: number;
  serialNumber: string;
  deviceName: string;
  model?: string;
  ipAddress?: string;
  deviceStatus: string;
  lastOnline?: string;
  isActive: boolean;
  location?: string;
  port?: number;
}

export interface User {
  id: number;
  pin: string;
  fullName: string;
  cardNumber?: string;
  email?: string;
  phoneNumber?: string;
  department?: string;
  position?: string;
  isActive: boolean;
  privilege: number;
  verifyMode: number;
  createdAt: string;
  updatedAt: string;
}

export interface AttendanceLog {
  id: number;
  deviceId: number;
  userId?: number;
  pin: string;
  verifyType?: number;
  verifyState: number;
  attendanceTime: string;
  workCode?: string;
  temperature?: number;
  maskStatus?: boolean;
  isProcessed: boolean;
  createdAt: string;
  device?: Device;
  user?: User;
}

export interface DeviceCommand {
  id: number;
  deviceId: number;
  commandType: string;
  commandData?: string;
  priority: number;
  status: string;
  responseData?: string;
  errorMessage?: string;
  createdAt: string;
  sentAt?: string;
  completedAt?: string;
}

export interface UserDeviceMapping {
  id: number;
  userId: number;
  deviceId: number;
  isSynced: boolean;
  lastSyncedAt?: string;
  syncStatus: string;
  errorMessage?: string;
  device?: Device;
}

export interface CreateUserRequest {
  pin: string;
  fullName: string;
  cardNumber?: string;
  password?: string;
  groupId?: number;
  privilege?: number;
  verifyMode?: number;
  email?: string;
  phoneNumber?: string;
  department?: string;
  position?: string;
}

export interface CreateDeviceRequest {
  serialNumber: string;
  deviceName: string;
  model?: string;
  ipAddress?: string;
  port?: number;
  location?: string;
}

export interface SendCommandRequest {
  commandType: string;
  commandData?: string;
  priority?: number;
}

export interface ApiResponse<T> {
  data?: T;
  message?: string;
  success: boolean;
}

export interface PaginatedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
