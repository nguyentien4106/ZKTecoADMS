using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;
using ZKTecoADMS.Infrastructure;
using ZKTecoADMS.Infrastructure.Repositories;

namespace ZKTecoADMS.Core.Services;


public class UserService(
    ZKTecoDbContext context,
    IDeviceService deviceService,
    IRepository<Device> deviceRepository,
    IRepository<User> userRepository,
    ILogger<UserService> logger) : IUserService
{
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await context.UserDevices.FindAsync(id);
    }

    public async Task<User?> GetUserByPinAsync(Guid deviceId, string pin)
    {
        return await context.UserDevices.FirstOrDefaultAsync(u => u.Pin == pin && u.DeviceId == deviceId);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.UserDevices.ToListAsync();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var existing = await GetUserByPinAsync(Guid.Empty, user.Pin);
        if (existing != null)
        {
            logger.LogWarning("{service} User with PIN {pin} already exists", "UserService", user.Pin);
        }

        await context.UserDevices.AddAsync(user);
        await context.SaveChangesAsync();

        logger.LogInformation("Created user: {UserName} (PIN: {PIN})", user.Name, user.Pin);
        return user;
    }

    public async Task<IEnumerable<User>> CreateUsersAsync(Guid deviceId, IEnumerable<User> newUsers)
    {
        // Filter out users with duplicate PINs
        var userPins = await context.UserDevices
            .Where(u => u.DeviceId == deviceId)
            .Select(u => u.Pin)
            .ToListAsync();
        
        var validUsers = newUsers.Where(u => !userPins.Contains(u.Pin)).ToList();
        if (validUsers.Count != 0)
        {
            await context.UserDevices.AddRangeAsync(validUsers);
            await context.SaveChangesAsync();
        }

        logger.LogInformation("Created {Count} users", validUsers.Count);
        return validUsers;
    }

    public async Task UpdateUserAsync(User user)
    {
        context.UserDevices.Update(user);
        user.UpdatedAt = DateTime.Now;
        await context.SaveChangesAsync();
        
        logger.LogInformation("Updated user: {UserName} (PIN: {PIN})", user.Name, user.Pin);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await GetUserByIdAsync(userId);
        if (user != null)
        {
            logger.LogInformation("Deleted user: {UserName} (PIN: {PIN})", user.Name, user.Pin);
        }
    }

    public async Task SyncUserToDeviceAsync(Guid userId, Guid deviceId)
    {
        var user = await context.UserDevices
            .Include(u => u.FingerprintTemplates)
            .Include(u => u.FaceTemplates)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

       
        // Create command to sync user info
        var userCommand = new DeviceCommand
        {
            DeviceId = deviceId,
            Command = $"DATA UPDATE USERINFO PIN={user.Pin}\tName={user.Name}\tPri={user.Privilege}\tPasswd={user.Password}\tCard={user.CardNumber}\tGrp={user.GroupId}",
            Priority = 3
        };
        await deviceService.CreateCommandAsync(userCommand);

        // Sync fingerprints
        foreach (var fp in user.FingerprintTemplates)
        {
            var fpCommand = new DeviceCommand
            {
                DeviceId = deviceId,
                Command = $"DATA UPDATE FP PIN={user.Pin}\tFID={fp.FingerIndex}\tSize={fp.TemplateSize}\tValid=1\tTMP={fp.Template}",
                Priority = 2
            };
            await deviceService.CreateCommandAsync(fpCommand);
        }

        // Sync faces
        foreach (var face in user.FaceTemplates)
        {
            var faceCommand = new DeviceCommand
            {
                DeviceId = deviceId,
                Command = $"DATA UPDATE FACE PIN={user.Pin}\tFID={face.FaceIndex}\tSize={face.TemplateSize}\tValid=1\tTMP={face.Template}",
                Priority = 2
            };
            await deviceService.CreateCommandAsync(faceCommand);
        }

        await context.SaveChangesAsync();

        logger.LogInformation("Initiated sync for user {UserName} to device {DeviceId}", user.Name, deviceId);
    }

    public async Task<bool> IsPinValidAsync(string pin, Guid deviceId)
    {
        try
        {
            await deviceRepository.GetSingleAsync(i => i.Id == deviceId);
            return false;
        }
        catch (Exception)
        {
            return true;
        }
    }
    
}