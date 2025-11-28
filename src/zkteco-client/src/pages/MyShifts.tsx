import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { ShiftList } from '@/components/shifts/ShiftList';
import { ShiftRequestDialog } from '@/components/dialogs/ShiftRequestDialog';
import { ShiftProvider, useShiftContext } from '@/contexts/ShiftContext';


const MyShiftsContent = () => {
    const { 
        shifts, 
        isLoading, 
        setDialogMode,
        handleEdit,
        handleDelete,
    } = useShiftContext();

    return (
        <div>
            <PageHeader
                title="My Shifts"
                description="Manage your shift schedules"
                action={
                    <Button onClick={() => setDialogMode('create')}>
                        <Plus className="w-4 h-4 mr-2" />
                        Request Shift
                    </Button>
                }
            />
             <div className="mt-6">
                <ShiftList
                    shifts={shifts}
                    isLoading={isLoading}
                    showEmployeeInfo={false}
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                />
            </div>
            
            <ShiftRequestDialog />
        </div>
    );
};

export const MyShifts = () => {
    return (
        <ShiftProvider>
            <MyShiftsContent />
        </ShiftProvider>
    );
};
