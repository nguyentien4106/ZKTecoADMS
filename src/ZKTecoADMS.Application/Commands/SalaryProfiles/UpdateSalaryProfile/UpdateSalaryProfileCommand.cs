using ZKTecoADMS.Application.DTOs.SalaryProfiles;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.UpdateSalaryProfile;

public record UpdateSalaryProfileCommand(
    Guid Id,
    string Name,
    string? Description,
    Domain.Enums.SalaryRateType RateType,
    decimal Rate,
    string Currency,
    decimal? OvertimeMultiplier,
    decimal? HolidayMultiplier,
    decimal? NightShiftMultiplier,
    // Base Salary Configuration
    int? StandardHoursPerDay,
    // Leave & Attendance Rules
    string? WeeklyOffDays,
    int? PaidLeaveDays,
    int? UnpaidLeaveDays,
    // Allowances
    decimal? MealAllowance,
    decimal? TransportAllowance,
    decimal? HousingAllowance,
    decimal? ResponsibilityAllowance,
    decimal? AttendanceBonus,
    decimal? PhoneSkillShiftAllowance,
    // Overtime Configuration
    decimal? OTRateWeekday,
    decimal? OTRateWeekend,
    decimal? OTRateHoliday,
    decimal? NightShiftRate,
    // Health Insurance
    bool? HasHealthInsurance,
    decimal? HealthInsuranceRate,
    bool IsActive
) : ICommand<AppResponse<SalaryProfileDto>>;
