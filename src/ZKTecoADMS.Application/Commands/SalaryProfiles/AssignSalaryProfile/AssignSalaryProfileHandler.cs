using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile;

public class AssignSalaryProfileHandler(
    IEmployeeSalaryProfileRepository employeeSalaryProfileRepository,
    ISalaryProfileRepository salaryProfileRepository,
    IRepository<Employee> employeeRepository) 
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

        // Deactivate other active profiles for this employee
        await employeeSalaryProfileRepository.DeactivateOtherProfilesAsync(
            request.EmployeeId, 
            employeeSalaryProfile.Id, 
            cancellationToken);

        // Reload with details
        var result = await employeeSalaryProfileRepository.GetByIdWithDetailsAsync(employeeSalaryProfile.Id, cancellationToken);

        var dto = new EmployeeSalaryProfileDto
        {
            Id = result!.Id,
            EmployeeId = result.EmployeeId,
            EmployeeName = result.Employee.Name,
            SalaryProfileId = result.SalaryProfileId,
            SalaryProfile = new SalaryProfileDto
            {
                Id = result.SalaryProfile.Id,
                Name = result.SalaryProfile.Name,
                Description = result.SalaryProfile.Description,
                RateType = result.SalaryProfile.RateType,
                RateTypeName = result.SalaryProfile.RateType.ToString(),
                Rate = result.SalaryProfile.Rate,
                Currency = result.SalaryProfile.Currency,
                OvertimeMultiplier = result.SalaryProfile.OvertimeMultiplier,
                HolidayMultiplier = result.SalaryProfile.HolidayMultiplier,
                NightShiftMultiplier = result.SalaryProfile.NightShiftMultiplier,
                IsActive = result.SalaryProfile.IsActive,
                CreatedAt = result.SalaryProfile.CreatedAt,
                UpdatedAt = result.SalaryProfile.UpdatedAt ?? result.SalaryProfile.CreatedAt
            },
            EffectiveDate = result.EffectiveDate,
            EndDate = result.EndDate,
            IsActive = result.IsActive,
            Notes = result.Notes,
            CreatedAt = result.CreatedAt
        };

        return AppResponse<EmployeeSalaryProfileDto>.Success(dto);
    }
}
