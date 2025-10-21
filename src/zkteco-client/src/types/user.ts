

export interface CreateUserRequest {
    pin: string;
    fullName: string;
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
    fullName: string;
    cardNumber?: string;
    password?: string;
    privilege?: number;
    email?: string;
    phoneNumber?: string;
    department?: string;
    deviceId: string; 
}