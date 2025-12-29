using ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile.SalaryProfileStrategies;
using ZKTecoADMS.Application.DTOs.Benefits;

namespace ZKTecoADMS.Application.Commands.Benefits.AssignEmployee;

public class AssignBenefitHandler(
    IRepository<EmployeeBenefit> employeeBenefitRepository,
    IRepository<Benefit> benefitRepository,
    IRepository<Employee> employeeRepository,
    BenefitAssignmentStrategyFactory strategyFactory) 
    : ICommandHandler<AssignBenefitCommand, AppResponse<EmployeeBenefitDto>>
{
    public async Task<AppResponse<EmployeeBenefitDto>> Handle(AssignBenefitCommand request, CancellationToken cancellationToken)
    {
        // Validate employee exists
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken: cancellationToken);
        if (employee == null)
        {
            return AppResponse<EmployeeBenefitDto>.Error("Employee not found");
        }

        // Validate benefit exists
        var benefit = await benefitRepository.GetByIdAsync(request.BenefitId, cancellationToken: cancellationToken);
        if (benefit == null)
        {
            return AppResponse<EmployeeBenefitDto>.Error("Benefit not found");
        }

        if (!benefit.IsActive)
        {
            return AppResponse<EmployeeBenefitDto>.Error("Benefit is not active");
        }

        if((int)employee.EmploymentType != (int)benefit.RateType)
        {
            return AppResponse<EmployeeBenefitDto>.Error("Benefit type does not match employee employment type");
        }

        // Get the appropriate strategy for the benefit type
        var strategy = strategyFactory.GetStrategy(benefit.RateType);
        
        // Validate the assignment using the strategy
        var (isValid, errorMessage) = await strategy.ValidateAssignmentAsync(benefit, employee, cancellationToken);
        if (!isValid)
        {
            return AppResponse<EmployeeBenefitDto>.Error(errorMessage ?? "Validation failed");
        }

        var employeeBenefit = await strategy.ConfigEmployeeBenefitAsync(benefit, employee);
        if (employeeBenefit == null)
        {
            return AppResponse<EmployeeBenefitDto>.Error("Failed to configure employee benefit");
        }

        employeeBenefit.EffectiveDate = request.EffectiveDate;
        employeeBenefit.Notes = request.Notes;

        await employeeBenefitRepository.AddAsync(employeeBenefit, cancellationToken);

        var othersEmployeeBenefits = await employeeBenefitRepository.GetAllAsync(
            eb => eb.EmployeeId == request.EmployeeId && eb.Id != employeeBenefit.Id && eb.IsActive,
            cancellationToken: cancellationToken
        );

        // Deactivate other active benefits
        foreach (var otherBenefit in othersEmployeeBenefits)
        {
            otherBenefit.IsActive = false;
            otherBenefit.EndDate = DateTime.Now;
            
            await employeeBenefitRepository.UpdateAsync(otherBenefit, cancellationToken);
        }

        return AppResponse<EmployeeBenefitDto>.Success(employeeBenefit.Adapt<EmployeeBenefitDto>());
    }
}
