using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByPINAsync(string pin);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
    Task SyncUserToDeviceAsync(Guid userId, Guid deviceId);
    Task SyncUserToAllDevicesAsync(Guid userId);
    Task<IEnumerable<UserDeviceMapping>> GetUserDeviceMappingsAsync(Guid userId);
}