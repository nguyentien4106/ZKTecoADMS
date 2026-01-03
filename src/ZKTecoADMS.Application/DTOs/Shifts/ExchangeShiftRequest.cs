namespace ZKTecoADMS.Application.DTOs.Shifts;

public class ExchangeShiftRequest
{
    public Guid ShiftId { get; set; }
    
    public Guid TargetEmployeeId { get; set; }
    
    public string Reason { get; set; } = string.Empty;
}