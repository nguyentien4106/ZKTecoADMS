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
    bool IsActive
) : ICommand<AppResponse<SalaryProfileDto>>;
