using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.UpdateSalaryProfile;

public class UpdateSalaryProfileHandler(ISalaryProfileRepository repository) 
    : ICommandHandler<UpdateSalaryProfileCommand, AppResponse<SalaryProfileDto>>
{
    public async Task<AppResponse<SalaryProfileDto>> Handle(UpdateSalaryProfileCommand request, CancellationToken cancellationToken)
    {
        var salaryProfile = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (salaryProfile == null)
        {
            return AppResponse<SalaryProfileDto>.Error("Salary profile not found");
        }

        // Check if name is unique (excluding current profile)
        var isUnique = await repository.IsNameUniqueAsync(request.Name, request.Id, cancellationToken);
        if (!isUnique)
        {
            return AppResponse<SalaryProfileDto>.Error($"A salary profile with the name '{request.Name}' already exists");
        }

        salaryProfile.Name = request.Name;
        salaryProfile.Description = request.Description;
        salaryProfile.RateType = request.RateType;
        salaryProfile.Rate = request.Rate;
        salaryProfile.Currency = request.Currency;
        salaryProfile.OvertimeMultiplier = request.OvertimeMultiplier;
        salaryProfile.HolidayMultiplier = request.HolidayMultiplier;
        salaryProfile.NightShiftMultiplier = request.NightShiftMultiplier;
        salaryProfile.IsActive = request.IsActive;
        salaryProfile.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(salaryProfile, cancellationToken);

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
