using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles.GetAllSalaryProfiles;

public class GetAllSalaryProfilesHandler(ISalaryProfileRepository repository) 
    : IQueryHandler<GetAllSalaryProfilesQuery, AppResponse<List<SalaryProfileDto>>>
{
    public async Task<AppResponse<List<SalaryProfileDto>>> Handle(GetAllSalaryProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = request.ActiveOnly == true
            ? await repository.GetActiveProfilesAsync(cancellationToken)
            : (await repository.GetAllAsync(cancellationToken: cancellationToken)).ToList();

        var dtos = profiles.Select(p => new SalaryProfileDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            RateType = p.RateType,
            RateTypeName = p.RateType.ToString(),
            Rate = p.Rate,
            Currency = p.Currency,
            OvertimeMultiplier = p.OvertimeMultiplier,
            HolidayMultiplier = p.HolidayMultiplier,
            NightShiftMultiplier = p.NightShiftMultiplier,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt ?? p.CreatedAt
        }).ToList();

        return AppResponse<List<SalaryProfileDto>>.Success(dtos);
    }
}
