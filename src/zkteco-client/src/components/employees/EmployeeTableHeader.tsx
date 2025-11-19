import { TableHead, TableHeader, TableRow } from "@/components/ui/table";

export const EmployeeTableHeader = () => {
  return (
    <TableHeader>
      <TableRow>
        <TableHead>Device</TableHead>
        <TableHead>PIN</TableHead>
        <TableHead>Name</TableHead>
        <TableHead>Privilege</TableHead>
        <TableHead>Department</TableHead>
        <TableHead>Card Number</TableHead>
        <TableHead className="text-right">Actions</TableHead>
      </TableRow>
    </TableHeader>
  );
};
