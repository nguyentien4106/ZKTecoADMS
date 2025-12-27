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
            EmployeeName = profile.Employee.FirstName + " " + profile.Employee.LastName,
            EffectiveDate = profile.EffectiveDate,
            EndDate = profile.EndDate,
            IsActive = profile.IsActive,
            Notes = profile.Notes,
            CreatedAt = profile.CreatedAt
        };

        return AppResponse<EmployeeSalaryProfileDto>.Success(dto);
    }
}
