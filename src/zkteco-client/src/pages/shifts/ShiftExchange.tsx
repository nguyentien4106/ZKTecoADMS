import { PageHeader } from "@/components/PageHeader";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { shiftService } from "@/services/shiftService";
import { ShiftExchangeRequestDto, ShiftExchangeStatus } from "@/types/shift";
import { format } from "date-fns";
import { CheckCircle, XCircle, Clock, User, Calendar } from "lucide-react";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { toast } from "sonner";
import { formatDate } from "@/lib/utils";

const ShiftExchangeContent = () => {
    const [activeTab, setActiveTab] = useState<string>("incoming");
    const [rejectDialogOpen, setRejectDialogOpen] = useState(false);
    const [selectedRequest, setSelectedRequest] = useState<ShiftExchangeRequestDto | null>(null);
    const [rejectionReason, setRejectionReason] = useState("");
    const queryClient = useQueryClient();

    // Fetch exchange requests
    const { data: allRequests = [], isLoading } = useQuery({
        queryKey: ['exchangeRequests'],
        queryFn: () => shiftService.getMyExchangeRequests(false)
    });

    const { data: incomingRequests = [] } = useQuery({
        queryKey: ['exchangeRequests', 'incoming'],
        queryFn: () => shiftService.getMyExchangeRequests(true)
    });

    // Approve mutation
    const approveMutation = useMutation({
        mutationFn: (requestId: string) => shiftService.approveExchangeRequest(requestId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['exchangeRequests'] });
            toast.success('Exchange request approved successfully');
        },
        onError: () => {
            toast.error('Failed to approve exchange request');
        }
    });

    // Reject mutation
    const rejectMutation = useMutation({
        mutationFn: ({ requestId, reason }: { requestId: string, reason: string }) => 
            shiftService.rejectExchangeRequest(requestId, reason),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['exchangeRequests'] });
            toast.success('Exchange request rejected');
            setRejectDialogOpen(false);
            setRejectionReason("");
        },
        onError: () => {
            toast.error('Failed to reject exchange request');
        }
    });

    const handleApprove = (request: ShiftExchangeRequestDto) => {
        approveMutation.mutate(request.id);
    };

    const handleRejectClick = (request: ShiftExchangeRequestDto) => {
        setSelectedRequest(request);
        setRejectDialogOpen(true);
    };

    const handleReject = () => {
        if (selectedRequest && rejectionReason.trim()) {
            rejectMutation.mutate({
                requestId: selectedRequest.id,
                reason: rejectionReason
            });
        }
    };

    const renderExchangeRequest = (request: ShiftExchangeRequestDto, isIncoming: boolean) => {
        const isPending = request.status === ShiftExchangeStatus.Pending;
        const isApproved = request.status === ShiftExchangeStatus.Approved;
        const isRejected = request.status === ShiftExchangeStatus.Rejected;

        return (
            <div key={request.id} className="border rounded-lg p-4 space-y-3">
                <div className="flex items-start justify-between">
                    <div className="space-y-2 flex-1">
                        <div className="flex items-center gap-2">
                            <User className="w-4 h-4 text-muted-foreground" />
                            <span className="font-medium">
                                {isIncoming ? `From: ${request.requesterName}` : `To: ${request.targetEmployeeName}`}
                            </span>
                            <Badge variant={
                                isPending ? "secondary" :
                                isApproved ? "default" : "destructive"
                            }>
                                {request.status}
                            </Badge>
                        </div>
                        
                        {request.shift && (
                            <>
                                <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                    <Calendar className="w-4 h-4" />
                                    <span>{formatDate(new Date(request.shift.date))}</span>
                                </div>
                                <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                    <Clock className="w-4 h-4" />
                                    <span>
                                        {request.shift.startTime} - {request.shift.endTime}
                                    </span>
                                </div>
                            </>
                        )}
                        
                        <div className="text-sm">
                            <span className="font-medium">Reason: </span>
                            <span className="text-muted-foreground">{request.reason}</span>
                        </div>

                        {isRejected && request.rejectionReason && (
                            <div className="text-sm text-destructive">
                                <span className="font-medium">Rejection Reason: </span>
                                <span>{request.rejectionReason}</span>
                            </div>
                        )}
                    </div>

                    {isPending && isIncoming && (
                        <div className="flex gap-2 ml-4">
                            <Button
                                size="sm"
                                variant="outline"
                                className="text-green-600 hover:text-green-700"
                                onClick={() => handleApprove(request)}
                            >
                                <CheckCircle className="w-4 h-4 mr-1" />
                                Approve
                            </Button>
                            <Button
                                size="sm"
                                variant="outline"
                                className="text-red-600 hover:text-red-700"
                                onClick={() => handleRejectClick(request)}
                            >
                                <XCircle className="w-4 h-4 mr-1" />
                                Reject
                            </Button>
                        </div>
                    )}
                </div>
            </div>
        );
    };

    const pendingCount = incomingRequests.filter(r => r.status === ShiftExchangeStatus.Pending).length;

    return (
        <div className="space-y-6">
            <PageHeader
                title="Shift Exchange"
                description="Manage shift exchange requests"
            />

            <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
                <TabsList>
                    <TabsTrigger value="incoming">
                        Incoming Requests
                        {pendingCount > 0 && (
                            <Badge className="ml-2 bg-yellow-500 text-white">{pendingCount}</Badge>
                        )}
                    </TabsTrigger>
                    <TabsTrigger value="sent">
                        Sent Requests
                    </TabsTrigger>
                </TabsList>

                <TabsContent value="incoming" className="mt-6">
                    {isLoading ? (
                        <div className="flex justify-center items-center py-12">
                            <LoadingSpinner />
                        </div>
                    ) : incomingRequests.length === 0 ? (
                        <div className="rounded-md border p-8 text-center text-muted-foreground">
                            No incoming exchange requests
                        </div>
                    ) : (
                        <div className="space-y-4">
                            {incomingRequests.map(request => renderExchangeRequest(request, true))}
                        </div>
                    )}
                </TabsContent>

                <TabsContent value="sent" className="mt-6">
                    {isLoading ? (
                        <div className="flex justify-center items-center py-12">
                            <LoadingSpinner />
                        </div>
                    ) : allRequests.filter(r => r.status !== ShiftExchangeStatus.Pending || activeTab === 'sent').length === 0 ? (
                        <div className="rounded-md border p-8 text-center text-muted-foreground">
                            No sent exchange requests
                        </div>
                    ) : (
                        <div className="space-y-4">
                            {allRequests
                                .filter(r => r.requesterId === r.requesterId) // Filter sent requests
                                .map(request => renderExchangeRequest(request, false))}
                        </div>
                    )}
                </TabsContent>
            </Tabs>

            {/* Reject Dialog */}
            <AlertDialog open={rejectDialogOpen} onOpenChange={setRejectDialogOpen}>
                <AlertDialogContent>
                    <AlertDialogHeader>
                        <AlertDialogTitle>Reject Exchange Request</AlertDialogTitle>
                        <AlertDialogDescription>
                            Please provide a reason for rejecting this exchange request.
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <div className="py-4">
                        <Label htmlFor="rejection-reason" className="text-sm font-medium">
                            Rejection Reason
                        </Label>
                        <Textarea
                            id="rejection-reason"
                            placeholder="Enter the reason for rejection..."
                            value={rejectionReason}
                            onChange={(e) => setRejectionReason(e.target.value)}
                            className="mt-2"
                            rows={4}
                        />
                    </div>
                    <AlertDialogFooter>
                        <AlertDialogCancel onClick={() => setRejectionReason('')}>
                            Cancel
                        </AlertDialogCancel>
                        <AlertDialogAction 
                            onClick={handleReject}
                            disabled={!rejectionReason.trim()}
                        >
                            Reject
                        </AlertDialogAction>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialog>
        </div>
    );
};

export const ShiftExchange = () => {
    return <ShiftExchangeContent />;
};

export default ShiftExchange;
