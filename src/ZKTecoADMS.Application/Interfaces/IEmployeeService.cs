using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IEmployeeService
{
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<Employee?> GetEmployeeByPinAsync(Guid deviceId, string pin);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<IEnumerable<Employee>> CreateEmployeesAsync(Guid deviceId, IEnumerable<Employee> employees);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(Guid employeeId);
    Task SyncEmployeeToDeviceAsync(Guid employeeId, Guid deviceId);
    Task<bool> IsPinValidAsync(string pin, Guid deviceId);
}