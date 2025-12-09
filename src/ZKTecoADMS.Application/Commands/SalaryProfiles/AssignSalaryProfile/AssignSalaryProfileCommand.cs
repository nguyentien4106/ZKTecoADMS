using ZKTecoADMS.Application.DTOs.SalaryProfiles;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile;

public record AssignSalaryProfileCommand(
    Guid EmployeeId,
    Guid SalaryProfileId,
    DateTime EffectiveDate,
    string? Notes
) : ICommand<AppResponse<EmployeeSalaryProfileDto>>;
