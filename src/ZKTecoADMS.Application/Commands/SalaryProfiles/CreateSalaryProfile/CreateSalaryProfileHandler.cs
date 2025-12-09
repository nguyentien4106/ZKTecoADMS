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

        var salaryProfile = new SalaryProfile
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            RateType = request.RateType,
            Rate = request.Rate,
            Currency = request.Currency,
            OvertimeMultiplier = request.OvertimeMultiplier,
            HolidayMultiplier = request.HolidayMultiplier,
            NightShiftMultiplier = request.NightShiftMultiplier,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(salaryProfile, cancellationToken);

        var dto = new SalaryProfileDto
        {
            Id = salaryProfile.Id,
            Name = salaryProfile.Name,
            Description = salaryProfile.Description,
            RateType = salaryProfile.RateType,
            RateTypeName = salaryProfile.RateType.ToString(),
            Rate = salaryProfile.Rate,
            Currency = salaryProfile.Currency,
            OvertimeMultiplier = salaryProfile.OvertimeMultiplier,
            HolidayMultiplier = salaryProfile.HolidayMultiplier,
            NightShiftMultiplier = salaryProfile.NightShiftMultiplier,
            IsActive = salaryProfile.IsActive,
            CreatedAt = salaryProfile.CreatedAt,
            UpdatedAt = salaryProfile.UpdatedAt ?? salaryProfile.CreatedAt
        };

        return AppResponse<SalaryProfileDto>.Success(dto);
    }
}
