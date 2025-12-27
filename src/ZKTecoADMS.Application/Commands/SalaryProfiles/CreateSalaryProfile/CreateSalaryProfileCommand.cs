using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.CreateSalaryProfile;

public record CreateSalaryProfileCommand(
    string Name,
    string? Description,
    SalaryRateType RateType,
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
    TimeOnly? CheckIn,
    TimeOnly? CheckOut,
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
    decimal? HealthInsuranceRate
) : ICommand<AppResponse<SalaryProfileDto>>;
