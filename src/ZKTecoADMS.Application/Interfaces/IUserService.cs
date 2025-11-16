using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByPinAsync(Guid deviceId, string pin);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<IEnumerable<User>> CreateUsersAsync(Guid deviceId, IEnumerable<User> users);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
    Task SyncUserToDeviceAsync(Guid userId, Guid deviceId);
    Task<bool> IsPinValidAsync(string pin, Guid deviceId);
}