using MediatR;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Employees.UpdateEmployee;

public class UpdateEmployeeHandler(
    IRepository<Employee> employeeRepository) : IRequestHandler<UpdateEmployeeCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.Id);
        
        if (employee == null)
        {
            return AppResponse<bool>.Error("Employee not found");
        }

        // Check if employee code is being changed and if it already exists
        if (employee.EmployeeCode != request.EmployeeCode)
        {
            var existingEmployee = await employeeRepository.GetFirstOrDefaultAsync(
                e => e.EmployeeCode,
                e => e.EmployeeCode == request.EmployeeCode && e.Id != request.Id,
                null,
                cancellationToken);

            if (existingEmployee != null)
            {
                return AppResponse<bool>.Error($"Employee with code {request.EmployeeCode} already exists");
            }
        }

        employee.EmployeeCode = request.EmployeeCode;
        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Gender = request.Gender;
        employee.DateOfBirth = request.DateOfBirth;
        employee.PhotoUrl = request.PhotoUrl;
        employee.NationalIdNumber = request.NationalIdNumber;
        employee.NationalIdIssueDate = request.NationalIdIssueDate;
        employee.NationalIdIssuePlace = request.NationalIdIssuePlace;
        employee.PhoneNumber = request.PhoneNumber;
        employee.PersonalEmail = request.PersonalEmail;
        employee.CompanyEmail = request.CompanyEmail;
        employee.PermanentAddress = request.PermanentAddress;
        employee.TemporaryAddress = request.TemporaryAddress;
        employee.EmergencyContactName = request.EmergencyContactName;
        employee.EmergencyContactPhone = request.EmergencyContactPhone;
        employee.Department = request.Department;
        employee.Position = request.Position;
        employee.Level = request.Level;
        employee.JoinDate = request.JoinDate;
        employee.ProbationEndDate = request.ProbationEndDate;
        employee.WorkStatus = request.WorkStatus;
        employee.ResignationDate = request.ResignationDate;
        employee.ResignationReason = request.ResignationReason;
        employee.ApplicationUserId = request.ApplicationUserId;
        employee.UpdatedAt = DateTime.UtcNow;

        await employeeRepository.UpdateAsync(employee, cancellationToken);

        return AppResponse<bool>.Success(true);
    }
}
