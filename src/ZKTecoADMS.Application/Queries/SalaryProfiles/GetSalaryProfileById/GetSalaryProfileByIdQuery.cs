using ZKTecoADMS.Application.DTOs.SalaryProfiles;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles.GetSalaryProfileById;

public record GetSalaryProfileByIdQuery(Guid Id) : IQuery<AppResponse<SalaryProfileDto>>;
