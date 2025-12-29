using ZKTecoADMS.Application.DTOs.Benefits;

namespace ZKTecoADMS.Application.Queries.Benefits.GetBenefitById;

public class GetBenefitByIdHandler(IRepository<Benefit> repository) 
    : IQueryHandler<GetBenefitByIdQuery, AppResponse<BenefitDto>>
{
    public async Task<AppResponse<BenefitDto>> Handle(GetBenefitByIdQuery request, CancellationToken cancellationToken)
    {
        var benefit = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (benefit == null)
        {
            return AppResponse<BenefitDto>.Error("Salary profile not found");
        }

        return AppResponse<BenefitDto>.Success(benefit.Adapt<BenefitDto>());
    }
}
