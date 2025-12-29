using ZKTecoADMS.Application.DTOs.Benefits;

namespace ZKTecoADMS.Application.Commands.Benefits.Update;

public class UpdateSalaryProfileHandler(
    IRepository<Benefit> repository
    ) : ICommandHandler<UpdateSalaryProfileCommand, AppResponse<BenefitDto>>
{
    public async Task<AppResponse<BenefitDto>> Handle(UpdateSalaryProfileCommand request, CancellationToken cancellationToken)
    {
        var salaryProfile = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (salaryProfile == null)
        {
            return AppResponse<BenefitDto>.Error("Salary profile not found");
        }

        // Check if name is unique (excluding current profile)
        var isUnique = await repository.ExistsAsync(b => b.Name == request.Name && b.Id != request.Id, cancellationToken);
        if (isUnique)
        {
            return AppResponse<BenefitDto>.Error($"A salary profile with the name '{request.Name}' already exists");
        }

        salaryProfile.Name = request.Name;
        salaryProfile.Description = request.Description;
        salaryProfile.Rate = request.Rate;
        salaryProfile.Currency = request.Currency;
        salaryProfile.OvertimeMultiplier = request.OvertimeMultiplier;
        salaryProfile.HolidayMultiplier = request.HolidayMultiplier;
        salaryProfile.NightShiftMultiplier = request.NightShiftMultiplier;
        salaryProfile.CheckIn = request.CheckIn;
        salaryProfile.CheckOut = request.CheckOut;

        // Base Salary Configuration
        salaryProfile.StandardHoursPerDay = request.StandardHoursPerDay;
        
        // Leave & Attendance Rules
        salaryProfile.WeeklyOffDays = request.WeeklyOffDays;
        salaryProfile.PaidLeaveDays = request.PaidLeaveDays;
        salaryProfile.UnpaidLeaveDays = request.UnpaidLeaveDays;
        // Allowances
        salaryProfile.MealAllowance = request.MealAllowance;
        salaryProfile.TransportAllowance = request.TransportAllowance;
        salaryProfile.HousingAllowance = request.HousingAllowance;
        salaryProfile.ResponsibilityAllowance = request.ResponsibilityAllowance;
        salaryProfile.AttendanceBonus = request.AttendanceBonus;
        salaryProfile.PhoneSkillShiftAllowance = request.PhoneSkillShiftAllowance;
        // Overtime Configuration
        salaryProfile.OTRateWeekday = request.OTRateWeekday;
        salaryProfile.OTRateWeekend = request.OTRateWeekend;
        salaryProfile.OTRateHoliday = request.OTRateHoliday;
        salaryProfile.NightShiftRate = request.NightShiftRate;
        // Health Insurance
        salaryProfile.HasHealthInsurance = request.HasHealthInsurance;
        salaryProfile.HealthInsuranceRate = request.HealthInsuranceRate;
        // Deductions

        await repository.UpdateAsync(salaryProfile, cancellationToken);

        var dto = salaryProfile.Adapt<BenefitDto>();

        return AppResponse<BenefitDto>.Success(dto);
    }
}
