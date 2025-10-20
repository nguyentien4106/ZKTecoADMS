namespace ZKTecoADMS.Application.DTOs.Users;

public record GetUsersByDevicesRequest(IEnumerable<Guid> DeviceIds);