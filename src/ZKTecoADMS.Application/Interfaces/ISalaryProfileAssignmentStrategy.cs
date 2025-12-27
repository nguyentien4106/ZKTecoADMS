using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

/// <summary>
/// Strategy interface for handling salary profile assignments based on rate type
/// </summary>
public interface ISalaryProfileAssignmentStrategy
{
    /// <summary>
    /// The rate type this strategy handles
    /// </summary>
    SalaryRateType RateType { get; }
    
    /// <summary>
    /// Validates the salary profile assignment
    /// </summary>
    Task<(bool IsValid, string? ErrorMessage)> ValidateAssignmentAsync(
        SalaryProfile salaryProfile, 
        Employee employee,
        CancellationToken cancellationToken = default);

    Task<SalaryProfile?> ConfigEmployeeWorkingInfoAsync(SalaryProfile salaryProfile, Employee employee);
}
