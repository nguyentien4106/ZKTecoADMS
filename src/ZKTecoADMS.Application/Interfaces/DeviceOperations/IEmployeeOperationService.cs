namespace ZKTecoADMS.Application.Interfaces;

/// <summary>
/// Service interface for parsing and processing employee data from device OPERLOG data.
/// </summary>
public interface IEmployeeOperationService
{
    /// <summary>
    /// Parses and processes employee data from device OPERLOG format.
    /// </summary>
    Task<List<Employee>> ProcessEmployeesFromDeviceAsync(Device device, string body);
}
