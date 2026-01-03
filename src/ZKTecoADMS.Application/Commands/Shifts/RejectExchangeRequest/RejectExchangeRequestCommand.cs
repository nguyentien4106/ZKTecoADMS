namespace ZKTecoADMS.Application.Commands.Shifts.RejectExchangeRequest;

public class RejectExchangeRequestCommand : ICommand<AppResponse<bool>>
{
    public Guid ExchangeRequestId { get; set; }
    
    public Guid TargetEmployeeId { get; set; }
    
    public string RejectionReason { get; set; } = string.Empty;
}
