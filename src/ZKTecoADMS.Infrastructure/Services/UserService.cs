using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Infrastructure;

namespace ZKTecoADMS.Core.Services;


public class UserService : IUserService
{
    private readonly ZKTecoDbContext _context;
    private readonly IDeviceService _deviceService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        ZKTecoDbContext context,
        IDeviceService deviceService,
        ILogger<UserService> logger)
    {
        _context = context;
        _deviceService = deviceService;
        _logger = logger;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.UserDevices.FindAsync(id);
    }

    public async Task<User?> GetUserByPINAsync(string pin)
    {
        return await _context.UserDevices.FirstOrDefaultAsync(u => u.PIN == pin);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.UserDevices.ToListAsync();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var existing = await GetUserByPINAsync(user.PIN);
        if (existing != null)
        {
            throw new InvalidOperationException($"User with PIN {user.PIN} already exists");
        }

        await _context.UserDevices.AddAsync(user);
        await _context.SaveChangesAsync();

        //_deviceService.CreateCommandAsync();
        _logger.LogInformation("Created user: {UserName} (PIN: {PIN})", user.FullName, user.PIN);
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.UserDevices.Update(user);
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Updated user: {UserName} (PIN: {PIN})", user.FullName, user.PIN);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await GetUserByIdAsync(userId);
        if (user != null)
        {
           
            
            _logger.LogInformation("Deleted user: {UserName} (PIN: {PIN})", user.FullName, user.PIN);
        }
    }

    public async Task SyncUserToDeviceAsync(Guid userId, Guid deviceId)
    {
        var user = await _context.UserDevices
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
            Command = $"DATA UPDATE USERINFO PIN={user.PIN}\tName={user.FullName}\tPri={user.Privilege}\tPasswd={user.Password}\tCard={user.CardNumber}\tGrp={user.GroupId}",
            Priority = 3
        };
        await _deviceService.CreateCommandAsync(userCommand);

        // Sync fingerprints
        foreach (var fp in user.FingerprintTemplates)
        {
            var fpCommand = new DeviceCommand
            {
                DeviceId = deviceId,
                Command = $"DATA UPDATE FP PIN={user.PIN}\tFID={fp.FingerIndex}\tSize={fp.TemplateSize}\tValid=1\tTMP={fp.Template}",
                Priority = 2
            };
            await _deviceService.CreateCommandAsync(fpCommand);
        }

        // Sync faces
        foreach (var face in user.FaceTemplates)
        {
            var faceCommand = new DeviceCommand
            {
                DeviceId = deviceId,
                Command = $"DATA UPDATE FACE PIN={user.PIN}\tFID={face.FaceIndex}\tSize={face.TemplateSize}\tValid=1\tTMP={face.Template}",
                Priority = 2
            };
            await _deviceService.CreateCommandAsync(faceCommand);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Initiated sync for user {UserName} to device {DeviceId}", user.FullName, deviceId);
    }

    public async Task SyncUserToAllDevicesAsync(Guid userId)
    {
        var devices = await _context.Devices
            .Where(d => d.IsActive)
            .ToListAsync();

        foreach (var device in devices)
        {
            await SyncUserToDeviceAsync(userId, device.Id);
        }
    }


}