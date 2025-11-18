import { ShiftTemplate } from '@/types/shift';
import { TableCell, TableRow } from '@/components/ui/table';
import { formatDateTime } from '@/lib/utils';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Pencil, Trash2 } from 'lucide-react';

interface ShiftTemplateRowProps {
    template: ShiftTemplate;
    onEdit?: (template: ShiftTemplate) => void;
    onDelete?: (id: string) => void;
}

export const ShiftTemplateRow = ({
    template,
    onEdit,
    onDelete
}: ShiftTemplateRowProps) => {
    return (
        <TableRow>
            <TableCell className="font-medium">{template.name}</TableCell>
            <TableCell className="whitespace-nowrap">{template.startTime}</TableCell>
            <TableCell className="whitespace-nowrap">{template.endTime}</TableCell>
            <TableCell className="whitespace-nowrap">{template.totalHours}</TableCell>
            <TableCell>
                <Badge variant={template.isActive ? "default" : "secondary"}>
                    {template.isActive ? "Active" : "Inactive"}
                </Badge>
            </TableCell>
            <TableCell className="text-sm text-muted-foreground whitespace-nowrap">
                {formatDateTime(template.createdAt)}
            </TableCell>
            <TableCell className="text-right">
                <div className="flex justify-end gap-2">
                    {onEdit && (
                        <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => onEdit(template)}
                        >
                            <Pencil className="h-4 w-4" />
                        </Button>
                    )}
                    {onDelete && (
                        <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => onDelete(template.id)}
                        >
                            <Trash2 className="h-4 w-4" />
                        </Button>
                    )}
                </div>
            </TableCell>
        </TableRow>
    );
};
