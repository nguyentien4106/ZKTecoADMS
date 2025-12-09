using ZKTecoADMS.Application.DTOs.SalaryProfiles;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.CreateSalaryProfile;

public record CreateSalaryProfileCommand(
    string Name,
    string? Description,
    Domain.Enums.SalaryRateType RateType,
    decimal Rate,
    string Currency,
    decimal? OvertimeMultiplier,
    decimal? HolidayMultiplier,
    decimal? NightShiftMultiplier
) : ICommand<AppResponse<SalaryProfileDto>>;
