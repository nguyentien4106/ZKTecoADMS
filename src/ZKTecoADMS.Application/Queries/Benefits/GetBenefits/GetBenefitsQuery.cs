
using ZKTecoADMS.Application.DTOs.Benefits;

namespace ZKTecoADMS.Application.Queries.Benefits.GetBenefits;

public record GetBenefitsQuery(int? SalaryRateType = null) : IQuery<AppResponse<List<BenefitDto>>>;
