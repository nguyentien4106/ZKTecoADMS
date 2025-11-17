using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Queries.Employees.GetEmployeeDevices;

public class GetEmployeeDevicesHandler(IRepository<Employee> userRepository) : IQueryHandler<GetEmployeeDevicesQuery, AppResponse<IEnumerable<EmployeeDto>>>
{
    public async Task<AppResponse<IEnumerable<EmployeeDto>>> Handle(GetEmployeeDevicesQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(
            i => request.DeviceIds.Contains(i.DeviceId),
            includeProperties: ["Device", "ApplicationUser"],
            orderBy: query => query.OrderByDescending(i => i.DeviceId).ThenBy(i => i.Pin),
            cancellationToken: cancellationToken);

        return AppResponse<IEnumerable<EmployeeDto>>.Success(users.Adapt<IEnumerable<EmployeeDto>>());
    }
}