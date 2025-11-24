using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IEmployeeService
{
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<Employee?> GetEmployeeByPinAsync(Guid deviceId, string pin);
    Task<IEnumerable<Employee>> CreateEmployeesAsync(Guid deviceId, IEnumerable<Employee> employees);
}