import { format } from "date-fns";
import { Eye } from "lucide-react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { useEmployeeSalaryProfiles } from "@/hooks/useSalaryProfiles";
import { SalaryRateType, EmployeeSalaryProfile } from "@/services/salaryProfileService";
import { useState } from "react";
import { EmployeeSalaryProfileDetailDialog } from "./EmployeeSalaryProfileDetailDialog";

const getRateTypeLabel = (type: SalaryRateType) => {
  switch (type) {
    case SalaryRateType.Hourly: return 'Hourly';
    case SalaryRateType.Monthly: return 'Monthly';
    default: return 'Unknown';
  }
};

export const EmployeeSalaryProfileTable = () => {
  const { data, isLoading } = useEmployeeSalaryProfiles({ pageSize: 100 });
  const [selectedProfile, setSelectedProfile] = useState<EmployeeSalaryProfile | null>(null);
  const [detailDialogOpen, setDetailDialogOpen] = useState(false);

  const handleViewDetails = (profile: EmployeeSalaryProfile) => {
    setSelectedProfile(profile);
    setDetailDialogOpen(true);
  };

  if (isLoading) {
    return <div className="text-center py-8">Loading...</div>;
  }

  const employeeSalaryProfiles = data?.items || [];

  return (
    <>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Employee</TableHead>
            <TableHead>Rate Type</TableHead>
            <TableHead>Rate</TableHead>
            <TableHead>Effective Date</TableHead>
            <TableHead>End Date</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Notes</TableHead>
            <TableHead>Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {employeeSalaryProfiles.length === 0 ? (
            <TableRow>
              <TableCell colSpan={9} className="text-center py-8 text-muted-foreground">
                No employee salary profiles found
              </TableCell>
            </TableRow>
          ) : (
            employeeSalaryProfiles.map((profile) => (
              <TableRow key={profile.id}>
                <TableCell className="font-medium">{profile.employeeName}</TableCell>
                <TableCell>
                  <Badge variant="outline">
                    {getRateTypeLabel(profile.rateType)}
                  </Badge>
                </TableCell>
                <TableCell>
                  {profile.rate.toLocaleString()} {profile.currency}
                </TableCell>
                <TableCell>
                  {format(new Date(profile.effectiveDate), 'MMM dd, yyyy')}
                </TableCell>
                <TableCell>
                  {profile.endDate
                    ? format(new Date(profile.endDate), 'MMM dd, yyyy')
                    : '-'}
                </TableCell>
                <TableCell>
                  <Badge variant={profile.isActive ? "default" : "secondary"}>
                    {profile.isActive ? 'Active' : 'Inactive'}
                  </Badge>
                </TableCell>
                <TableCell className="max-w-xs truncate">
                  {profile.notes || '-'}
                </TableCell>
                <TableCell>
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => handleViewDetails(profile)}
                  >
                    <Eye className="h-4 w-4" />
                  </Button>
                </TableCell>
              </TableRow>
            ))
          )}
        </TableBody>
      </Table>

      <EmployeeSalaryProfileDetailDialog
        open={detailDialogOpen}
        onOpenChange={setDetailDialogOpen}
        profile={selectedProfile}
      />
    </>
  );
};

