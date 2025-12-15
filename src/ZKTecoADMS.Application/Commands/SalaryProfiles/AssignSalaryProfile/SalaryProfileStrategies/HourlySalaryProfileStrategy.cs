using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile.SalaryProfileStrategies;

/// <summary>
/// Strategy for handling hourly salary profile assignments
/// </summary>
public class HourlySalaryProfileStrategy(
    IRepository<EmployeeWorkingInfo> employeeWorkingInfoRepository
) : ISalaryProfileAssignmentStrategy
{
    public SalaryRateType RateType => SalaryRateType.Hourly;

    public Task<(bool IsValid, string? ErrorMessage)> ValidateAssignmentAsync(
        Domain.Entities.SalaryProfile salaryProfile,
        Employee employee,
        CancellationToken cancellationToken = default)
    {
        // Validate hourly rate is positive
        if (salaryProfile.Rate <= 0)
        {
            return Task.FromResult<(bool, string?)>((false, "Hourly rate must be greater than zero"));
        }

        // Validate multipliers are reasonable
        if (salaryProfile.OvertimeMultiplier.HasValue && salaryProfile.OvertimeMultiplier.Value < 1)
        {
            return Task.FromResult<(bool, string?)>((false, "Overtime multiplier must be at least 1.0"));
        }

        if (salaryProfile.HolidayMultiplier.HasValue && salaryProfile.HolidayMultiplier.Value < 1)
        {
            return Task.FromResult<(bool, string?)>((false, "Holiday multiplier must be at least 1.0"));
        }

        if (salaryProfile.NightShiftMultiplier.HasValue && salaryProfile.NightShiftMultiplier.Value < 1)
        {
            return Task.FromResult<(bool, string?)>((false, "Night shift multiplier must be at least 1.0"));
        }

        return Task.FromResult<(bool, string?)>((true, null));
    }

    public async Task<SalaryProfile?> ConfigEmployeeWorkingInfoAsync(SalaryProfile salaryProfile, Employee employee)
    {
        // For hourly salary profiles, there may not be additional working info to configure
        await employeeWorkingInfoRepository.DeleteAsync(
            e => e.EmployeeId == employee.Id,
            cancellationToken: CancellationToken.None
        );
        
        return await Task.FromResult<SalaryProfile?>(null);
    }

}
