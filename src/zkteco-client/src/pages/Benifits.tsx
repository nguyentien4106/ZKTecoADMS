import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { Card } from "@/components/ui/card";
import { SalaryProfileTable } from "@/components/benefit/dialogs/BenefitsTable";
import { CreateSalaryProfileDialog } from "@/components/benefit/dialogs/CreateBenefitDialog";
import { EditSalaryProfileDialog } from "@/components/benefit/dialogs/EditBenefitDialog";
import { BenefitProvider, useSalaryProfileContext } from "@/contexts/BenefitContext";

const BenefitsContent = () => {
  const {
    handleOpenCreateDialog,
  } = useSalaryProfileContext();

  return (
    <div className="space-y-6">
      <PageHeader
        title="Benefit Profiles"
        action={
          <Button onClick={handleOpenCreateDialog}>
            <Plus className="w-4 h-4 mr-2" />
            Create Benefit Profile
          </Button>
        }
      />

      <Card className="p-4">
        <SalaryProfileTable />
      </Card>

      <CreateSalaryProfileDialog />
      <EditSalaryProfileDialog />
    </div>
  );
};

export const Benefits = () => {
  return (
    <BenefitProvider>
      <BenefitsContent />
    </BenefitProvider>
  );
};
