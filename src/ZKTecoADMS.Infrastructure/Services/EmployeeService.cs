using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;
using ZKTecoADMS.Infrastructure;

namespace ZKTecoADMS.Core.Services;


public class EmployeeService(
    ZKTecoDbContext context,
    IDeviceService deviceService,
    IRepository<Device> deviceRepository,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<EmployeeService> logger) : IEmployeeService
{
    public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
    {
        return await context.Employees.FindAsync(id);
    }

    public async Task<Employee?> GetEmployeeByPinAsync(Guid deviceId, string pin)
    {
        return await context.Employees.FirstOrDefaultAsync(u => u.Pin == pin && u.DeviceId == deviceId);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await context.Employees.ToListAsync();
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        var existing = await GetEmployeeByPinAsync(Guid.Empty, employee.Pin);
        if (existing != null)
        {
            logger.LogWarning("{service} Employee with PIN {pin} already exists", "EmployeeService", employee.Pin);
        }

        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();

        logger.LogInformation("Created employee: {EmployeeName} (PIN: {PIN})", employee.Name, employee.Pin);
        return employee;
    }

    public async Task<IEnumerable<Employee>> CreateEmployeesAsync(Guid deviceId, IEnumerable<Employee> newEmployees)
    {
        // Filter out employees with duplicate PINs
        var employeePins = await context.Employees
            .Where(u => u.DeviceId == deviceId)
            .Select(u => u.Pin)
            .ToListAsync();
            
        var device = await deviceRepository.GetSingleAsync(d => d.Id == deviceId);
        
        if(device == null)
        {
            throw new ArgumentException("Device not found", nameof(deviceId));
        }

        var validEmployees = newEmployees.Where(u => !employeePins.Contains(u.Pin)).ToList();
        if (validEmployees.Count != 0)
        {
            await context.Employees.AddRangeAsync(validEmployees);
            await context.SaveChangesAsync();
        }
        await CreateEmployeeAccounts(validEmployees, device);

        logger.LogInformation("Created {Count} employees", validEmployees.Count);
        return validEmployees;
    }

    private async Task CreateEmployeeAccounts(IEnumerable<Employee> validEmployees, Device device)
    {
        foreach (var employee in validEmployees)
        {
            var name = $"{device.SerialNumber}_{employee.Pin}".Replace(" ", "").ToLower();
            var user = new ApplicationUser
            {
                UserName = name,
                Email = $"{name}@gmail.com",
                FirstName = employee.Name,
                LastName = "",
                EmailConfirmed = true,
                PhoneNumber = "",
                PhoneNumberConfirmed = true,
                Employee = employee,
                ManagerId = device.ManagerId,
                CreatedBy = "System",
                Created = DateTime.Now
            };

            var result = await userManager.CreateAsync(user, configuration["DefaultEmployeeAccountPassword"] ?? "Tien100600@");

            if (result.Succeeded)
            {
                logger.LogInformation("Created application user for employee: {EmployeeName} (PIN: {PIN})", employee.Name, employee.Pin);
                await userManager.AddToRoleAsync(user, "Employee");
            }
            else
            {
                logger.LogError("Failed to create application user for employee: {EmployeeName} (PIN: {PIN}). Errors: {Errors}",
                    employee.Name,
                    employee.Pin,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }   
        }
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        context.Employees.Update(employee);
        employee.UpdatedAt = DateTime.Now;
        await context.SaveChangesAsync();
        
        logger.LogInformation("Updated employee: {EmployeeName} (PIN: {PIN})", employee.Name, employee.Pin);
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        var employee = await GetEmployeeByIdAsync(employeeId);
        if (employee != null)
        {
            logger.LogInformation("Deleted employee: {EmployeeName} (PIN: {PIN})", employee.Name, employee.Pin);
        }
    }

    public async Task SyncEmployeeToDeviceAsync(Guid employeeId, Guid deviceId)
    {
        var employee = await context.Employees
            .Include(u => u.FingerprintTemplates)
            .Include(u => u.FaceTemplates)
            .FirstOrDefaultAsync(u => u.Id == employeeId);

        if (employee == null)
        {
            throw new ArgumentException("Employee not found", nameof(employeeId));
        }

       
        // Create command to sync employee info
        var employeeCommand = new DeviceCommand
        {
            DeviceId = deviceId,
            Command = $"DATA UPDATE USERINFO PIN={employee.Pin}\tName={employee.Name}\tPri={employee.Privilege}\tPasswd={employee.Password}\tCard={employee.CardNumber}\tGrp={employee.GroupId}",
            Priority = 3
        };
        await deviceService.CreateCommandAsync(employeeCommand);

        // Sync fingerprints
        foreach (var fp in employee.FingerprintTemplates)
        {
            var fpCommand = new DeviceCommand
            {
                DeviceId = deviceId,
                Command = $"DATA UPDATE FP PIN={employee.Pin}\tFID={fp.FingerIndex}\tSize={fp.TemplateSize}\tValid=1\tTMP={fp.Template}",
                Priority = 2
            };
            await deviceService.CreateCommandAsync(fpCommand);
        }

        // Sync faces
        foreach (var face in employee.FaceTemplates)
        {
            var faceCommand = new DeviceCommand
            {
                DeviceId = deviceId,
                Command = $"DATA UPDATE FACE PIN={employee.Pin}\tFID={face.FaceIndex}\tSize={face.TemplateSize}\tValid=1\tTMP={face.Template}",
                Priority = 2
            };
            await deviceService.CreateCommandAsync(faceCommand);
        }

        await context.SaveChangesAsync();

        logger.LogInformation("Initiated sync for employee {EmployeeName} to device {DeviceId}", employee.Name, deviceId);
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