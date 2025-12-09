using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles.GetEmployeeSalaryProfile;

public class GetEmployeeSalaryProfileHandler(IEmployeeSalaryProfileRepository repository) 
    : IQueryHandler<GetEmployeeSalaryProfileQuery, AppResponse<EmployeeSalaryProfileDto>>
{
    public async Task<AppResponse<EmployeeSalaryProfileDto>> Handle(GetEmployeeSalaryProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetActiveByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        if (profile == null)
        {
            return AppResponse<EmployeeSalaryProfileDto>.Error("No active salary profile found for this employee");
        }

        var dto = new EmployeeSalaryProfileDto
        {
            Id = profile.Id,
            EmployeeId = profile.EmployeeId,
            EmployeeName = profile.Employee.Name,
            SalaryProfileId = profile.SalaryProfileId,
            SalaryProfile = new SalaryProfileDto
            {
                Id = profile.SalaryProfile.Id,
                Name = profile.SalaryProfile.Name,
                Description = profile.SalaryProfile.Description,
                RateType = profile.SalaryProfile.RateType,
                RateTypeName = profile.SalaryProfile.RateType.ToString(),
                Rate = profile.SalaryProfile.Rate,
                Currency = profile.SalaryProfile.Currency,
                OvertimeMultiplier = profile.SalaryProfile.OvertimeMultiplier,
                HolidayMultiplier = profile.SalaryProfile.HolidayMultiplier,
                NightShiftMultiplier = profile.SalaryProfile.NightShiftMultiplier,
                IsActive = profile.SalaryProfile.IsActive,
                CreatedAt = profile.SalaryProfile.CreatedAt,
                UpdatedAt = profile.SalaryProfile.UpdatedAt ?? profile.SalaryProfile.CreatedAt
            },
            EffectiveDate = profile.EffectiveDate,
            EndDate = profile.EndDate,
            IsActive = profile.IsActive,
            Notes = profile.Notes,
            CreatedAt = profile.CreatedAt
        };

        return AppResponse<EmployeeSalaryProfileDto>.Success(dto);
    }
}
