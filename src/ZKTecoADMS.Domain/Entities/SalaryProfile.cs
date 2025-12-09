using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class SalaryProfile : AuditableEntity<Guid>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public SalaryRateType RateType { get; set; }

    [Required]
    public decimal Rate { get; set; }

    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    public decimal? OvertimeMultiplier { get; set; }

    public decimal? HolidayMultiplier { get; set; }

    public decimal? NightShiftMultiplier { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<EmployeeSalaryProfile> EmployeeSalaryProfiles { get; set; } = new List<EmployeeSalaryProfile>();
}
