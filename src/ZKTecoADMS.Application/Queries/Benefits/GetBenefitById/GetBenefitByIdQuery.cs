using ZKTecoADMS.Application.DTOs.Benefits;

namespace ZKTecoADMS.Application.Queries.Benefits.GetBenefitById;

public record GetBenefitByIdQuery(Guid Id) : IQuery<AppResponse<BenefitDto>>;
