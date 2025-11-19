import { accountService } from "@/services/accountService";
import { useQuery } from "@tanstack/react-query";

export const useEmployeesByManager = (enabled: boolean = true) => {

    return useQuery({
        queryKey: ['employeeAccountsByManager'],
        queryFn: () => accountService.getEmployeeAccountsByManager(),
        enabled,
    });
}