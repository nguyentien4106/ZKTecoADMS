import { TableCell, TableRow } from "@/components/ui/table";

export const EmployeeTableLoading = () => {
  return (
    <TableRow>
      <TableCell colSpan={9} className="h-48">
        <div className="flex items-center justify-center">
          <div className="flex flex-col items-center gap-2">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
            <span className="text-muted-foreground">Loading users...</span>
          </div>
        </div>
      </TableCell>
    </TableRow>
  );
};
