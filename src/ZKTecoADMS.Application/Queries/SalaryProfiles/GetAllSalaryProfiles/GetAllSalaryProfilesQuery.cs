using ZKTecoADMS.Application.DTOs.SalaryProfiles;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles.GetAllSalaryProfiles;

public record GetAllSalaryProfilesQuery(bool? ActiveOnly = null) : IQuery<AppResponse<List<SalaryProfileDto>>>;
