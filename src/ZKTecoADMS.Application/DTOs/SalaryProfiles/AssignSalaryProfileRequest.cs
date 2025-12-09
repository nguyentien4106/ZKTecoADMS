namespace ZKTecoADMS.Application.DTOs.SalaryProfiles;

public class AssignSalaryProfileRequest
{
    public Guid EmployeeId { get; set; }
    public Guid SalaryProfileId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string? Notes { get; set; }
}
