using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles.GetSalaryProfileById;

public class GetSalaryProfileByIdHandler(ISalaryProfileRepository repository) 
    : IQueryHandler<GetSalaryProfileByIdQuery, AppResponse<SalaryProfileDto>>
{
    public async Task<AppResponse<SalaryProfileDto>> Handle(GetSalaryProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (profile == null)
        {
            return AppResponse<SalaryProfileDto>.Error("Salary profile not found");
        }

        var dto = new SalaryProfileDto
        {
            Id = profile.Id,
            Name = profile.Name,
            Description = profile.Description,
            RateType = profile.RateType,
            RateTypeName = profile.RateType.ToString(),
            Rate = profile.Rate,
            Currency = profile.Currency,
            OvertimeMultiplier = profile.OvertimeMultiplier,
            HolidayMultiplier = profile.HolidayMultiplier,
            NightShiftMultiplier = profile.NightShiftMultiplier,
            IsActive = profile.IsActive,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt ?? profile.CreatedAt
        };

        return AppResponse<SalaryProfileDto>.Success(dto);
    }
}
