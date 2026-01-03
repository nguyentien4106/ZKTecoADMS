namespace ZKTecoADMS.Application.Commands.Shifts.ExchangeShift;

public class ExchangeShiftCommand : ICommand<AppResponse<bool>>
{
    public Guid TargetEmployeeId { get; set; }
    
    public Guid ShiftId { get; set; }
    
    public string Reason { get; set; } = string.Empty;
    
    public Guid CurrentEmployeeId { get; set; }
}