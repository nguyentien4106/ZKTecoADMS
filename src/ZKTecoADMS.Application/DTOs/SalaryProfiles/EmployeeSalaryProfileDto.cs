using ZKTecoADMS.Application.DTOs.DeviceUsers;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.SalaryProfiles;

public class EmployeeSalaryProfileDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;

    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }

    // Snapshot of salary configuration at the time of assignment
    public SalaryRateType RateType { get; set; }
    public decimal Rate { get; set; }
    public int? StandardHoursPerDay { get; set; }
    public string Currency { get; set; } = "VND";
    public decimal? OvertimeMultiplier { get; set; }
    public decimal? HolidayMultiplier { get; set; }
    public decimal? NightShiftMultiplier { get; set; }

    // Base Salary Configuration
    public decimal? SalaryPerDay { get; set; }
    
    // Leave & Attendance Rules
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

    public DateTime CreatedAt { get; set; }
}
