using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile.SalaryProfileStrategies;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile;

public class AssignSalaryProfileHandler(
    IEmployeeSalaryProfileRepository employeeSalaryProfileRepository,
    ISalaryProfileRepository salaryProfileRepository,
    IRepository<Employee> employeeRepository,
    SalaryProfileStrategyFactory strategyFactory) 
    : ICommandHandler<AssignSalaryProfileCommand, AppResponse<EmployeeSalaryProfileDto>>
{
    public async Task<AppResponse<EmployeeSalaryProfileDto>> Handle(AssignSalaryProfileCommand request, CancellationToken cancellationToken)
    {
        // Validate employee exists
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken: cancellationToken);
        if (employee == null)
        {
            return AppResponse<EmployeeSalaryProfileDto>.Error("Employee not found");
        }

        // Validate salary profile exists
        var salaryProfile = await salaryProfileRepository.GetByIdAsync(request.SalaryProfileId, cancellationToken: cancellationToken);
        if (salaryProfile == null)
        {
            return AppResponse<EmployeeSalaryProfileDto>.Error("Salary profile not found");
        }

        if (!salaryProfile.IsActive)
        {
            return AppResponse<EmployeeSalaryProfileDto>.Error("Salary profile is not active");
        }

        if((int)employee.EmploymentType != (int)salaryProfile.RateType)
        {
            return AppResponse<EmployeeSalaryProfileDto>.Error("Salary profile type does not match employee employment type");
        }

        // Get the appropriate strategy for the salary profile type
        var strategy = strategyFactory.GetStrategy(salaryProfile.RateType);
        
        // Validate the assignment using the strategy
        var (isValid, errorMessage) = await strategy.ValidateAssignmentAsync(salaryProfile, employee, cancellationToken);
        if (!isValid)
        {
            return AppResponse<EmployeeSalaryProfileDto>.Error(errorMessage ?? "Validation failed");
        }

        // Create new employee salary profile
        var employeeSalaryProfile = salaryProfile.Adapt<EmployeeSalaryProfile>();
        employeeSalaryProfile.EmployeeId = request.EmployeeId;
        employeeSalaryProfile.EffectiveDate = request.EffectiveDate;
        employeeSalaryProfile.Notes = request.Notes;
        employeeSalaryProfile.Id = Guid.NewGuid();

        await employeeSalaryProfileRepository.AddAsync(employeeSalaryProfile, cancellationToken);

        await employeeSalaryProfileRepository.DeactivateOtherProfilesAsync(
            request.EmployeeId, 
            employeeSalaryProfile.Id, 
            cancellationToken);
            
        return AppResponse<EmployeeSalaryProfileDto>.Success(salaryProfile.Adapt<EmployeeSalaryProfileDto>());
    }
}
