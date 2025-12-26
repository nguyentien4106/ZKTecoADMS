using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;

namespace ZKTecoADMS.Domain.Entities;

public class EmployeeSalaryProfile : AuditableEntity<Guid>
{
    [Required]
    public Guid EmployeeId { get; set; }
    public virtual DeviceUser Employee { get; set; } = null!;

    [Required]
    public Guid SalaryProfileId { get; set; }
    public virtual SalaryProfile SalaryProfile { get; set; } = null!;

    [Required]
    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
