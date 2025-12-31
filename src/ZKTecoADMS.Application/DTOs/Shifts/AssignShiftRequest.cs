namespace ZKTecoADMS.Application.DTOs.Shifts;

public class AssignShiftRequest : CreateShiftRequest
{
    public Guid EmployeeId { get; set; }
}