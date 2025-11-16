

export interface CreateUserRequest {
    pin: string;
    name: string;
    cardNumber?: string;
    password?: string;
    privilege?: number;
    email?: string;
    phoneNumber?: string;
    department?: string;
    deviceIds?: string[];
}

export interface UpdateUserRequest {
    userId: string
    pin: string;
    name: string;
    cardNumber?: string;
    password?: string;
    privilege?: number;
    email?: string;
    phoneNumber?: string;
    department?: string;
    deviceId: string; 
}

export interface User {
  id: string;
  pin: string;
  name: string;
  cardNumber?: string;
  email?: string;
  phoneNumber?: string;
  department?: string;
  isActive: boolean;
  privilege: 0 | 1 | 2 | 14;
  verifyMode: number;
  createdAt: string;
  updatedAt: string;
  deviceId: string;
  deviceName?: string;
}