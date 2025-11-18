import { CreateShiftTemplateRequest, ShiftTemplate, UpdateShiftTemplateRequest } from "@/types/shift";
import { apiService } from "./api";

export const shiftTemplateService = {
     // Shift Template endpoints
    getShiftTemplates: async () => {
        return await apiService.get<ShiftTemplate[]>('/api/shifttemplates');
    },

    createShiftTemplate: async (data: CreateShiftTemplateRequest) => {
        return await apiService.post<ShiftTemplate>('/api/shifttemplates', data);
    },

    updateShiftTemplate: async (id: string, data: UpdateShiftTemplateRequest) => {
        return await apiService.put<ShiftTemplate>(`/api/shifttemplates/${id}`, data);
    },

    deleteShiftTemplate: async (id: string) => {
        return await apiService.delete<boolean>(`/api/shifttemplates/${id}`);
    },
}