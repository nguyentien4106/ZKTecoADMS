import { Button } from "@/components/ui/button";
import { Edit, Trash2 } from "lucide-react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { SalaryProfile, SalaryRateType } from "@/services/salaryProfileService";

interface SalaryProfileTableProps {
  profiles: SalaryProfile[];
  loading: boolean;
  onEdit: (profile: SalaryProfile) => void;
  onDelete: (id: string) => void;
}

const getRateTypeLabel = (type: SalaryRateType) => {
  switch (type) {
    case SalaryRateType.Hourly: return 'Hourly';
    case SalaryRateType.Monthly: return 'Monthly';
    default: return 'Unknown';
  }
};

export const SalaryProfileTable = ({
  profiles,
  loading,
  onEdit,
  onDelete,
}: SalaryProfileTableProps) => {
  if (loading) {
    return <div className="text-center py-8">Loading...</div>;
  }

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Name</TableHead>
          <TableHead>Profile Type</TableHead>
          <TableHead>Rate/Salary</TableHead>
          <TableHead>Currency</TableHead>
          <TableHead>OT Multiplier</TableHead>
          <TableHead>Status</TableHead>
          <TableHead className="text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {profiles.length === 0 ? (
          <TableRow>
            <TableCell colSpan={7} className="text-center py-8 text-muted-foreground">
              No salary profiles found
            </TableCell>
          </TableRow>
        ) : (
          profiles.map((profile) => (
            <TableRow key={profile.id}>
              <TableCell className="font-medium">{profile.name}</TableCell>
              <TableCell>
                <Badge variant="outline">{getRateTypeLabel(profile.rateType)}</Badge>
              </TableCell>
              <TableCell>
                {profile.rateType === SalaryRateType.Monthly 
                  ? `${profile.rate.toLocaleString()} / month`
                  : `${profile.rate.toLocaleString()} / hour`
                }
              </TableCell>
              <TableCell>{profile.currency}</TableCell>
              <TableCell>
                {profile.rateType === SalaryRateType.Hourly && profile.overtimeMultiplier
                  ? `${profile.overtimeMultiplier}x`
                  : 'N/A'}
              </TableCell>
              <TableCell>
                <Badge variant={profile.isActive ? "default" : "secondary"}>
                  {profile.isActive ? 'Active' : 'Inactive'}
                </Badge>
              </TableCell>
              <TableCell className="text-right">
                <div className="flex justify-end gap-2">
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => onEdit(profile)}
                  >
                    <Edit className="w-4 h-4" />
                  </Button>
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => onDelete(profile.id)}
                  >
                    <Trash2 className="w-4 h-4" />
                  </Button>
                </div>
              </TableCell>
            </TableRow>
          ))
        )}
      </TableBody>
    </Table>
  );
};
