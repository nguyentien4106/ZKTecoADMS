import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { ShiftList } from '@/components/shifts/ShiftList';
import { CreateShiftDialog } from '@/components/dialogs/CreateShiftDialog';
import { UpdateShiftDialog } from '@/components/dialogs/UpdateShiftDialog';
import { ShiftProvider, useShiftContext } from '@/contexts/ShiftContext';


const MyShiftsContent = () => {
    const { 
        shifts, 
        isLoading, 
        selectedShift, 
        setCreateDialogOpen ,
        handleEdit,
        handleDelete,
    } = useShiftContext();

    return (
        <div>
            <PageHeader
                title="My Shifts"
                description="Manage your shift schedules"
                action={
                    <Button onClick={() => setCreateDialogOpen(true)}>
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
            <CreateShiftDialog />

            {selectedShift && (
                <UpdateShiftDialog />
            )}
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
