using MediatR;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.Employees.GetEmployeeById;

public class GetEmployeeByIdHandler(IRepository<Employee> employeeRepository) 
    : IRequestHandler<GetEmployeeByIdQuery, AppResponse<EmployeeDto>>
{
    public async Task<AppResponse<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.Id);
        
        if (employee == null)
        {
            return AppResponse<EmployeeDto>.Error("Employee not found");
        }

        var employeeDto = new EmployeeDto
        {
            Id = employee.Id,
            EmployeeCode = employee.EmployeeCode,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Gender = employee.Gender,
            DateOfBirth = employee.DateOfBirth,
            PhotoUrl = employee.PhotoUrl,
            NationalIdNumber = employee.NationalIdNumber,
            NationalIdIssueDate = employee.NationalIdIssueDate,
            NationalIdIssuePlace = employee.NationalIdIssuePlace,
            PhoneNumber = employee.PhoneNumber,
            PersonalEmail = employee.PersonalEmail,
            CompanyEmail = employee.CompanyEmail,
            PermanentAddress = employee.PermanentAddress,
            TemporaryAddress = employee.TemporaryAddress,
            EmergencyContactName = employee.EmergencyContactName,
            EmergencyContactPhone = employee.EmergencyContactPhone,
            Department = employee.Department,
            Position = employee.Position,
            Level = employee.Level,
            EmploymentType = employee.EmploymentType,
            JoinDate = employee.JoinDate,
            ProbationEndDate = employee.ProbationEndDate,
            WorkStatus = employee.WorkStatus,
            ResignationDate = employee.ResignationDate,
            ResignationReason = employee.ResignationReason,
            ApplicationUserId = employee.ApplicationUserId
        };

        return AppResponse<EmployeeDto>.Success(employeeDto);
    }
}
