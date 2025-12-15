using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.CreateSalaryProfile;

public class CreateSalaryProfileHandler(ISalaryProfileRepository repository) 
    : ICommandHandler<CreateSalaryProfileCommand, AppResponse<SalaryProfileDto>>
{
    public async Task<AppResponse<SalaryProfileDto>> Handle(CreateSalaryProfileCommand request, CancellationToken cancellationToken)
    {
        // Check if name is unique
        var isUnique = await repository.IsNameUniqueAsync(request.Name, null, cancellationToken);
        if (!isUnique)
        {
            return AppResponse<SalaryProfileDto>.Error($"A salary profile with the name '{request.Name}' already exists");
        }

        var salaryProfile = request.Adapt<SalaryProfile>();
        salaryProfile.IsActive = true;

        await repository.AddAsync(salaryProfile, cancellationToken);

        var dto = salaryProfile.Adapt<SalaryProfileDto>();

        return AppResponse<SalaryProfileDto>.Success(dto);
    }
}
