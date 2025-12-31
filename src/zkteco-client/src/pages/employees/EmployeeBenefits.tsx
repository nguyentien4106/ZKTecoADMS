import { PageHeader } from "@/components/PageHeader";
import { Card } from "@/components/ui/card";
import { EmployeeSalaryProfileTable } from "@/components/benefit/employees-benefit/EmployeeBenefitsTable";
import { AssignSalaryToEmployeeDialog } from "@/components/benefit/employees-benefit/AssignSalaryToEmployeeDialog";
import { BenefitProvider } from "@/contexts/BenefitContext";

const EmployeeBenefitsContent = () => {
  return (
    <div className="space-y-6">
      <PageHeader title="Employee Benefits" />

      <Card className="p-4">
        <EmployeeSalaryProfileTable />
      </Card>

      <AssignSalaryToEmployeeDialog />
    </div>
  );
};

export const EmployeeBenefits = () => {
  return (
    <BenefitProvider>
      <EmployeeBenefitsContent />
    </BenefitProvider>
  );
};

export default EmployeeBenefits;
