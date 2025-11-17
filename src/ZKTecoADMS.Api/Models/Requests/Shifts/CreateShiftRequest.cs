namespace ZKTecoADMS.Api.Models.Requests.Shifts;

public class CreateShiftRequest
{
    public Guid EmployeeId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Description { get; set; }
}
