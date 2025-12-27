using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class EmployeeSalaryProfile : AuditableEntity<Guid>
{
    [Required]
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    [Required]
    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public decimal? BalancedPaidLeaveDays { get; set; }

    public decimal? UsedPaidLeaveDays { get; set; }

    // Snapshot of salary configuration at the time of assignment
    // This allows historical tracking even if the profile changes
    
    [Required]
    public SalaryRateType RateType { get; set; }

    [Required]
    public decimal Rate { get; set; }

    public int? StandardHoursPerDay { get; set; }

    [MaxLength(10)]
    public string Currency { get; set; } = "VND";

    public decimal? OvertimeMultiplier { get; set; }

    public decimal? HolidayMultiplier { get; set; }

    public decimal? NightShiftMultiplier { get; set; }

    // Base Salary Configuration
    public decimal? SalaryPerDay { get; set; }
    
    // Leave & Attendance Rules
    [MaxLength(100)]
    public string? WeeklyOffDays { get; set; }
    
    public decimal? PaidLeaveDays { get; set; }
    
    public decimal? UnpaidLeaveDays { get; set; }

    public TimeOnly? CheckIn { get; set; }

    public TimeOnly? CheckOut { get; set; }
    
    // Allowances
    public decimal? MealAllowance { get; set; }
    
    public decimal? TransportAllowance { get; set; }
    
    public decimal? HousingAllowance { get; set; }
    
    public decimal? ResponsibilityAllowance { get; set; }
    
    public decimal? AttendanceBonus { get; set; }
    
    public decimal? PhoneSkillShiftAllowance { get; set; }

    // Overtime Configuration
    public decimal? OTRateWeekday { get; set; }
    
    public decimal? OTRateWeekend { get; set; }
    
    public decimal? OTRateHoliday { get; set; }
    
    public decimal? NightShiftRate { get; set; }
    
    // Health Insurance
    public bool? HasHealthInsurance { get; set; }
    
    public decimal? HealthInsuranceRate { get; set; }
}
