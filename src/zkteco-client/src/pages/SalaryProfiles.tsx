import { useState } from "react";
import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { Label } from "@/components/ui/label";
import { Badge } from "@/components/ui/badge";
import { Card } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { SalaryProfileTable } from "@/components/salary-profile/SalaryProfileTable";
import { EmployeeSalaryProfileTable } from "@/components/salary-profile/EmployeeSalaryProfileTable";
import { CreateSalaryProfileDialog } from "@/components/salary-profile/CreateSalaryProfileDialog";
import { EditSalaryProfileDialog } from "@/components/salary-profile/EditSalaryProfileDialog";
import { AssignSalaryToEmployeeDialog } from "@/components/salary-profile/AssignSalaryToEmployeeDialog";
import { SalaryProfileProvider, useSalaryProfileContext } from "@/contexts/SalaryProfileContext";

const SalaryProfilesContent = () => {
  const {
    profiles,
    showActiveOnly,
    setShowActiveOnly,
    handleOpenCreateDialog,
  } = useSalaryProfileContext();

  const [activeTab, setActiveTab] = useState("profiles");

  return (
    <div className="space-y-6">
      <PageHeader
        title="Salary Profiles"
        description="Manage salary profiles and employee assignments"
        action={
          activeTab === "profiles" && (
            <Button onClick={handleOpenCreateDialog}>
              <Plus className="w-4 h-4 mr-2" />
              Create Profile
            </Button>
          )
        }
      />

      <Tabs value={activeTab} onValueChange={setActiveTab}>
        <TabsList>
          <TabsTrigger value="profiles">Salary Profiles</TabsTrigger>
          <TabsTrigger value="assignments">Employee Assignments</TabsTrigger>
        </TabsList>

        <TabsContent value="profiles" className="mt-6">
          <Card className="p-4">
            <div className="flex items-center justify-between mb-4">
              <div className="flex items-center space-x-2">
                <input
                  type="checkbox"
                  checked={showActiveOnly}
                  onChange={(e) => setShowActiveOnly(e.target.checked)}
                  id="active-only"
                  className="h-4 w-4"
                />
                <Label htmlFor="active-only">Show active only</Label>
              </div>
              <Badge variant="outline" className="text-sm">
                {profiles?.length || 0} profile{profiles?.length !== 1 ? 's' : ''}
              </Badge>
            </div>

            <SalaryProfileTable />
          </Card>
        </TabsContent>

        <TabsContent value="assignments" className="mt-6">
          <Card className="p-4">
            <EmployeeSalaryProfileTable />
          </Card>
        </TabsContent>
      </Tabs>

      <CreateSalaryProfileDialog />
      <EditSalaryProfileDialog />
      <AssignSalaryToEmployeeDialog />
    </div>
  );
};

export const SalaryProfiles = () => {
  return (
    <SalaryProfileProvider>
      <SalaryProfilesContent />
    </SalaryProfileProvider>
  );
};
