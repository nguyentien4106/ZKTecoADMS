using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

/// <summary>
/// Service interface for parsing and processing user data from device OPERLOG data.
/// </summary>
public interface IUserOperationService
{
    /// <summary>
    /// Parses and processes user data from device OPERLOG format.
    /// </summary>
    Task<List<User>> ProcessUsersFromDeviceAsync(Device device, string body);
}
