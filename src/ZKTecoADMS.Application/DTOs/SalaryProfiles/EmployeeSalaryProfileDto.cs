using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.DTOs.SalaryProfiles;

public class EmployeeSalaryProfileDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public Guid SalaryProfileId { get; set; }
    public SalaryProfileDto SalaryProfile { get; set; } = null!;
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
