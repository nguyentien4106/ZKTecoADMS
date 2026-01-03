namespace ZKTecoADMS.Application.Commands.Shifts.ApproveExchangeRequest;

public class ApproveExchangeRequestCommand : ICommand<AppResponse<bool>>
{
    public Guid ExchangeRequestId { get; set; }
    
    public Guid TargetEmployeeId { get; set; }
}
