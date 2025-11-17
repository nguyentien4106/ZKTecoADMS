namespace ZKTecoADMS.Application.DTOs.Employees;

public record GetEmployeesByDevicesRequest(IEnumerable<Guid> DeviceIds);