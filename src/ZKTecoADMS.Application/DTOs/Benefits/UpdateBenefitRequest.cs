using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.Benefits;

public class UpdateSalaryProfileRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public decimal? OvertimeMultiplier { get; set; }
    public decimal? HolidayMultiplier { get; set; }
    public decimal? NightShiftMultiplier { get; set; }
    
    // Base Salary Configuration (Monthly profiles)
    public int? StandardHoursPerDay { get; set; }

    // Leave & Attendance Rules
    public string? WeeklyOffDays { get; set; }
    public int? PaidLeaveDays { get; set; }
    public int? UnpaidLeaveDays { get; set; }

    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }

    // Allowances
    public decimal? MealAllowance { get; set; }
    public decimal? TransportAllowance { get; set; }
    public decimal? HousingAllowance { get; set; }
    public decimal? ResponsibilityAllowance { get; set; }
    public decimal? AttendanceBonus { get; set; }
    public decimal? PhoneSkillShiftAllowance { get; set; }

    // Overtime Configuration (for Monthly profiles)
    public decimal? OTRateWeekday { get; set; }
    public decimal? OTRateWeekend { get; set; }
    public decimal? OTRateHoliday { get; set; }
    public decimal? NightShiftRate { get; set; }

    // Health Insurance
    public bool? HasHealthInsurance { get; set; }
    public decimal? HealthInsuranceRate { get; set; }

    public bool IsActive { get; set; }
}
