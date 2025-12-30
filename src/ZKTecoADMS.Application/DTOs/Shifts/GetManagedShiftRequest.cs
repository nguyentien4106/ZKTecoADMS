namespace ZKTecoADMS.Application.DTOs.Shifts;

public class GetManagedShiftRequest
{
    public List<Guid> EmployeeIds { get; set; } = [];
    
    public int Month { get; set; }
    
    public int Year { get; set; }
}