import { apiService } from './api';
import type { 
    LeaveRequest, 
    CreateLeaveRequest, 
    RejectLeaveRequest 
} from '@/types/leave';

export const leaveService = {
    // Employee endpoints
    getMyLeaves: async () => {
        return await apiService.get<LeaveRequest[]>('/api/leaves/my-leaves');
    },

    createLeave: async (data: CreateLeaveRequest) => {
        return await apiService.post<LeaveRequest>('/api/leaves', data);
    },

    cancelLeave: async (id: string) => {
        return await apiService.delete<boolean>(`/api/leaves/${id}`);
    },

    // Manager endpoints
    getPendingLeaves: async () => {
        return await apiService.get<LeaveRequest[]>('/api/leaves/pending');
    },

    getAllLeaves: async () => {
        return await apiService.get<LeaveRequest[]>('/api/leaves');
    },

    approveLeave: async (id: string) => {
        return await apiService.post<LeaveRequest>(`/api/leaves/${id}/approve`, {});
    },

    rejectLeave: async (id: string, data: RejectLeaveRequest) => {
        return await apiService.post<LeaveRequest>(`/api/leaves/${id}/reject`, data);
    },
};
