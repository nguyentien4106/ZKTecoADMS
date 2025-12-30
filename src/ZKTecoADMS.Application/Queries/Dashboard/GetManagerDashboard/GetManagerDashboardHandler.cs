using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.Dashboard.GetManagerDashboard;

public class GetManagerDashboardHandler(
    IRepository<Shift> shiftRepository,
    IRepository<Leave> leaveRepository,
    IRepository<Attendance> attendanceRepository,
    UserManager<ApplicationUser> userManager,
    ILogger<GetManagerDashboardHandler> logger
) : IQueryHandler<GetManagerDashboardQuery, AppResponse<ManagerDashboardDto>>
{
    public async Task<AppResponse<ManagerDashboardDto>> Handle(GetManagerDashboardQuery request, CancellationToken cancellationToken)
    {
            var startOfDay = request.Date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            // Get all employees managed by this manager
            // var managedUsers = await userRepository.GetAllAsync(
            //     filter: u => u.ManagerId == request.ManagerUserId,
            //     cancellationToken: cancellationToken);
            var managedUsers = await userManager.Users.Where(u => u.ManagerId == request.ManagerUserId
            ).ToListAsync(cancellationToken);

            var managedUserIds = managedUsers.Select(e => e.Id).ToList();

            if (!managedUserIds.Any())
            {
                return AppResponse<ManagerDashboardDto>.Success(new ManagerDashboardDto
                {
                    AttendanceRate = new AttendanceRateDto()
                });
            }

            // Get all shifts for today for managed employees  
            var todayShifts = (await shiftRepository.GetAllAsync(
                filter: s => managedUserIds.Contains(s.EmployeeId) &&
                             s.StartTime >= startOfDay &&
                             s.StartTime <= endOfDay &&
                             s.Status == ShiftStatus.Approved,
                includeProperties: new[] { "ApplicationUser", "ApplicationUser.Employee" },
                cancellationToken: cancellationToken)).ToList();

            // Get all approved leaves for today that are associated with shifts
            return new();
        }
    }

