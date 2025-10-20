// ==========================================
// src/types/index.ts
// ==========================================
export interface Device {
  id: string;
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
  id: string;
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
  deviceId: string;
  deviceName?: string;
}

export interface AttendanceLog {
  id: string;
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
  id: string;
  deviceId: number;
  command: string;
  priority: number;
  status: string;
  responseData?: string;
  errorMessage?: string;
  createdAt: string;
  sentAt?: string;
  completedAt?: string;
}

export interface UserDeviceMapping {
  id: string;
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
  DeviceIds?: string[];
}

export interface CreateDeviceRequest {
  serialNumber: string;
  deviceName: string;
  model?: string;
  ipAddress?: string;
  port?: number;
  location?: string;
  applicationUserId: string
}

export interface SendCommandRequest {
  commandType: string;
  commandData?: string;
  priority?: number;
}

export interface AppResponse<T> {
  data: T;
  errors: string[];
  isSuccess: boolean;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  previousPageNumber?: number;
  nextPageNumber?: number;
}
