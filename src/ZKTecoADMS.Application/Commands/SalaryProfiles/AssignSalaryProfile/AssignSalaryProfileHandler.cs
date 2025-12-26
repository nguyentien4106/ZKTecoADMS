using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile.SalaryProfileStrategies;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile;

public class AssignSalaryProfileHandler(
    IEmployeeSalaryProfileRepository employeeSalaryProfileRepository,
    ISalaryProfileRepository salaryProfileRepository,
    IRepository<DeviceUser> employeeRepository,
    IRepository<EmployeeWorkingInfo> employeeWorkingInfoRepository,
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

        // Get the appropriate strategy for the salary profile type
        var strategy = strategyFactory.GetStrategy(salaryProfile.RateType);
        
        // Validate the assignment using the strategy
        var (isValid, errorMessage) = await strategy.ValidateAssignmentAsync(salaryProfile, employee, cancellationToken);
        if (!isValid)
        {
            return AppResponse<EmployeeSalaryProfileDto>.Error(errorMessage ?? "Validation failed");
        }

        // Create new employee salary profile
        var employeeSalaryProfile = new EmployeeSalaryProfile
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            SalaryProfileId = request.SalaryProfileId,
            EffectiveDate = request.EffectiveDate,
            IsActive = true,
            Notes = request.Notes,
            CreatedAt = DateTime.Now
        };

        await employeeSalaryProfileRepository.AddAsync(employeeSalaryProfile, cancellationToken);

        await employeeSalaryProfileRepository.DeactivateOtherProfilesAsync(
            request.EmployeeId, 
            employeeSalaryProfile.Id, 
            cancellationToken);
            
        salaryProfile = await strategy.ConfigEmployeeWorkingInfoAsync(salaryProfile, employee);

        return AppResponse<EmployeeSalaryProfileDto>.Success(salaryProfile.Adapt<EmployeeSalaryProfileDto>());
        // Deactivate other active profiles for this employee


        // // Configure or update EmployeeWorkingInfo using the strategy
        // var workingInfo = await employeeWorkingInfoRepository.GetSingleAsync(
        //     wi => wi.EmployeeId == request.EmployeeId, 
        //     cancellationToken: cancellationToken);

        // if (workingInfo == null)
        // {
        //     // Create new working info
        //     workingInfo = new EmployeeWorkingInfo
        //     {
        //         Id = Guid.NewGuid(),
        //         EmployeeId = request.EmployeeId,
        //         EmployeeUserId = employee.ApplicationUserId,
        //         BalancedLeaveDays = 0,
        //         TotalLeaveDays = 0,
        //         BalancedLateEarlyLeaveMinutes = 0
        //     };
            
        //     strategy.ConfigureEmployeeWorkingInfo(workingInfo, salaryProfile);
        //     await employeeWorkingInfoRepository.AddAsync(workingInfo, cancellationToken);
        // }
        // else
        // {
        //     // Update existing working info
        //     strategy.ConfigureEmployeeWorkingInfo(workingInfo, salaryProfile);
        //     await employeeWorkingInfoRepository.UpdateAsync(workingInfo, cancellationToken);
        // }

        // // Reload with details
        // var result = await employeeSalaryProfileRepository.GetByIdWithDetailsAsync(employeeSalaryProfile.Id, cancellationToken);

        // // Use the strategy to enrich the DTO with rate-type specific fields
        // var salaryProfileDto = strategy.EnrichSalaryProfileDto(result!.SalaryProfile);
        
        // var dto = new EmployeeSalaryProfileDto
        // {
        //     Id = result.Id,
        //     EmployeeId = result.EmployeeId,
        //     EmployeeName = result.Employee.Name,
        //     SalaryProfileId = result.SalaryProfileId,
        //     SalaryProfile = salaryProfileDto,
        //     EffectiveDate = result.EffectiveDate,
        //     EndDate = result.EndDate,
        //     IsActive = result.IsActive,
        //     Notes = result.Notes,
        //     CreatedAt = result.CreatedAt
        // };

        // return AppResponse<EmployeeSalaryProfileDto>.Success(dto);
    }
}
