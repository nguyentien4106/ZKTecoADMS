import { PaginatedResponse, PaginationRequest } from '@/types';
import { apiService, buildQueryParams } from './api';
import type { 
    CreateShiftRequest, 
    UpdateShiftRequest,
    UpdateShiftTimesRequest,
    RejectShiftRequest,
    Shift,
    ShiftStatus,
    ShiftManagementFilter,
    ExchangeShiftRequest,
    ShiftExchangeRequestDto
} from '@/types/shift';

export const shiftService = {
    // Employee endpoints
    getMyShifts: async (month: number, year: number, status?: ShiftStatus) => {
        const params: any = { Month: month, Year: year };
        if (status !== undefined && status !== 'all' as any) {
            params.Status = status;
        }
        const queryString = buildQueryParams(params);
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
    getPendingShifts: async (paginationRequest: PaginationRequest) => {
        const queryString = buildQueryParams(paginationRequest);
        return await apiService.get<PaginatedResponse<Shift>>('/api/shifts/pending' + (queryString ? `?${queryString}` : ''));
    },

    getManagedShifts: async (paginationRequest: PaginationRequest, filters: ShiftManagementFilter) => {
        const queryString = buildQueryParams({...paginationRequest });
        return await apiService.post<PaginatedResponse<Shift>>('/api/shifts/managed' + (queryString ? `?${queryString}` : ''), filters);
    },

    approveShift: async (id: string) => {
        return await apiService.post<Shift>(`/api/shifts/${id}/approve`, {});
    },

    approveShifts: async (ids: string[]) => {
        return await apiService.post<Shift[]>('/api/shifts/approve-multiple', { ids });
    },

    rejectShift: async (id: string, data: RejectShiftRequest) => {
        return await apiService.post<Shift>(`/api/shifts/${id}/reject`, data);
    },

    rejectShifts: async (ids: string[], rejectionReason: string) => {
        return await apiService.post<Shift[]>('/api/shifts/reject-multiple', { ids, rejectionReason });
    },

    updateShiftTimes: async (id: string, data: UpdateShiftTimesRequest) => {
        return await apiService.put<Shift>(`/api/shifts/${id}/times`, data);
    },

    assignShift: async (data: CreateShiftRequest & { employeeId: string }) => {
        return await apiService.post<Shift>('/api/shifts/assign', data);
    },

    exchangeShift: async (data: ExchangeShiftRequest) => {
        return await apiService.post<boolean>('/api/shifts/exchange', data);
    },

    getMyExchangeRequests: async (incomingOnly: boolean = false) => {
        const queryString = buildQueryParams({ incomingOnly });
        return await apiService.get<ShiftExchangeRequestDto[]>('/api/shifts/exchange/my-requests' + (queryString ? `?${queryString}` : ''));
    },

    approveExchangeRequest: async (exchangeRequestId: string) => {
        return await apiService.post<boolean>(`/api/shifts/exchange/${exchangeRequestId}/approve`, {});
    },

    rejectExchangeRequest: async (exchangeRequestId: string, rejectionReason: string) => {
        return await apiService.post<boolean>(`/api/shifts/exchange/${exchangeRequestId}/reject`, { rejectionReason });
    }
};
