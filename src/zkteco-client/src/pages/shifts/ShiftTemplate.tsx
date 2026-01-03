import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { ShiftTemplateDialog } from '@/components/shiftTemplates/dialogs/ShiftTemplateDialog';
import { ShiftManagementProvider, useShiftManagementContext } from '@/contexts/ShiftManagementContext';
import { ShiftTemplateList } from "@/components/shiftTemplates/ShiftTemplateList";

const ShiftTemplateContent = () => {
    const {
        templates,
        isLoading,
        setCreateTemplateDialogOpen,
        handleEditTemplateClick,
        handleDeleteTemplate,
    } = useShiftManagementContext();

    return (
        <div>
            {/* <ShiftTemplateHeader /> */}
            <PageHeader 
                title="Templates"
                action={
                    <Button onClick={() => setCreateTemplateDialogOpen(true)}>
                        <Plus className="h-4 w-4 mr-2" />
                        Create Template
                    </Button>
                }
            />
            
            <div className="mt-6">
                <ShiftTemplateList
                    templates={templates}
                    isLoading={isLoading}
                    onEdit={handleEditTemplateClick}
                    onDelete={handleDeleteTemplate}
                />
            </div>

            <ShiftTemplateDialog />
        </div>
    );
};

export const ShiftTemplate = () => {
    return (
        <ShiftManagementProvider>
            <ShiftTemplateContent />
        </ShiftManagementProvider>
    );
};
