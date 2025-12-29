using ZKTecoADMS.Application.DTOs.Benefits;

namespace ZKTecoADMS.Application.Commands.Benefits.Create;

public class CreateSalaryProfileHandler(
    IRepository<Benefit> repository
    ) : ICommandHandler<CreateSalaryProfileCommand, AppResponse<BenefitDto>>
{
    public async Task<AppResponse<BenefitDto>> Handle(CreateSalaryProfileCommand request, CancellationToken cancellationToken)
    {
        // Check if name is unique
        
        var isUnique = await repository.ExistsAsync(b => b.Name == request.Name, cancellationToken);
        if (isUnique)
        {
            return AppResponse<BenefitDto>.Error($"A salary profile with the name '{request.Name}' already exists");
        }

        var salaryProfile = request.Adapt<Benefit>();
        salaryProfile.IsActive = true;

        await repository.AddAsync(salaryProfile, cancellationToken);

        var dto = salaryProfile.Adapt<BenefitDto>();

        return AppResponse<BenefitDto>.Success(dto);
    }
}
