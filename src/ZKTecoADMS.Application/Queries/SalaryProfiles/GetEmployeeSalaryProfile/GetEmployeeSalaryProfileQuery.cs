using ZKTecoADMS.Application.DTOs.SalaryProfiles;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles.GetEmployeeSalaryProfile;

public record GetEmployeeSalaryProfileQuery(Guid EmployeeId) : IQuery<AppResponse<EmployeeSalaryProfileDto>>;
