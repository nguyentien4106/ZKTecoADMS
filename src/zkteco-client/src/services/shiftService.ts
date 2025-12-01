import { apiService, buildQueryParams } from './api';
import type { 
    CreateShiftRequest, 
    UpdateShiftRequest, 
    RejectShiftRequest,
    Shift,
    ShiftStatus
} from '@/types/shift';

export const shiftService = {
    // Employee endpoints
    getMyShifts: async (status?: ShiftStatus, employeeUserId?: string) => {
        const queryString = buildQueryParams({ status, employeeUserId });
        return await apiService.get<Shift[]>('/api/shifts/my-shifts' + (queryString ? `?${queryString}` : ''));
    },

    createShift: async (data: CreateShiftRequest) => {
        return await apiService.post<Shift>('/api/shifts', data);
    },

    updateShift: async (id: string, data: UpdateShiftRequest) => {
        return await apiService.put<Shift>(`/api/shifts/${id}`, data);
    },

    deleteShift: async (id: string) => {
        return await apiService.delete<boolean>(`/api/shifts/${id}`);
    },

    // Manager endpoints
    getPendingShifts: async () => {
        return await apiService.get<Shift[]>('/api/shifts/pending');
    },

    getManagedShifts: async () => {
        return await apiService.get<Shift[]>('/api/shifts/managed');
    },

    approveShift: async (id: string) => {
        return await apiService.post<Shift>(`/api/shifts/${id}/approve`, {});
    },

    rejectShift: async (id: string, data: RejectShiftRequest) => {
        return await apiService.post<Shift>(`/api/shifts/${id}/reject`, data);
    },

    assignShift: async (data: CreateShiftRequest & { employeeUserId: string }) => {
        return await apiService.post<Shift>('/api/shifts', data);
    },

};
