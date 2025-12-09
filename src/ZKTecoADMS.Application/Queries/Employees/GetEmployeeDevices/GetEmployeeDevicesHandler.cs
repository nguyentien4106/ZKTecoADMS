using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Queries.Employees.GetEmployeeDevices;

public class GetEmployeeDevicesHandler(IRepository<Employee> userRepository) : IQueryHandler<GetEmployeeDevicesQuery, AppResponse<IEnumerable<EmployeeDto>>>
{
    public async Task<AppResponse<IEnumerable<EmployeeDto>>> Handle(GetEmployeeDevicesQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(
            i => request.DeviceIds.Contains(i.DeviceId),
            includeProperties: ["Device", "ApplicationUser", "SalaryProfiles.SalaryProfile"],
            orderBy: query => query.OrderByDescending(i => i.DeviceId).ThenBy(i => i.Pin),
            cancellationToken: cancellationToken);

        var employeeDtos = users.Select(emp => {
            var dto = emp.Adapt<EmployeeDto>();
            
            // Get the active salary profile for this employee
            var activeSalaryProfile = emp.SalaryProfiles
                .Where(sp => sp.IsActive)
                .OrderByDescending(sp => sp.EffectiveDate)
                .FirstOrDefault();
            
            if (activeSalaryProfile != null && activeSalaryProfile.SalaryProfile != null)
            {
                dto.CurrentSalaryProfile = new CurrentSalaryProfileDto
                {
                    Id = activeSalaryProfile.Id,
                    SalaryProfileId = activeSalaryProfile.SalaryProfileId,
                    ProfileName = activeSalaryProfile.SalaryProfile.Name,
                    Rate = activeSalaryProfile.SalaryProfile.Rate,
                    Currency = activeSalaryProfile.SalaryProfile.Currency,
                    RateTypeName = activeSalaryProfile.SalaryProfile.RateType.ToString(),
                    EffectiveDate = activeSalaryProfile.EffectiveDate,
                    IsActive = activeSalaryProfile.IsActive
                };
            }
            
            return dto;
        });

        return AppResponse<IEnumerable<EmployeeDto>>.Success(employeeDtos);
    }
}