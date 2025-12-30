using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Application.Extensions;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Dashboard.GetEmployeeDashboard;

public class GetEmployeeDashboardHandler(
    IRepository<Shift> shiftRepository,
    IRepository<Attendance> attendanceRepository,
    UserManager<ApplicationUser> userManager,
    IShiftService shiftService
) : IQueryHandler<GetEmployeeDashboardQuery, AppResponse<EmployeeDashboardDto>>
{
    public async Task<AppResponse<EmployeeDashboardDto>> Handle(
        GetEmployeeDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Where(u => u.Id == request.UserId).Include(u => u.Employee).FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            return AppResponse<EmployeeDashboardDto>.Fail("User not found");
        }
        var (todayShift, nextShift) = await shiftService.GetTodayShiftAndNextShiftAsync(request.UserId, cancellationToken);

        var currentAttendance = new AttendanceInfoDto
        {
            CheckInTime = todayShift?.CheckInAttendance?.AttendanceTime ?? null,
            CheckOutTime = todayShift?.CheckOutAttendance?.AttendanceTime ?? null,
            Status = todayShift == null ? "no-shift" :
                     todayShift.CheckInAttendanceId == null ? "not-started" :
                     todayShift.CheckOutAttendanceId == null ? "checked-in" : "checked-out",
        };
        var dashboardData = new EmployeeDashboardDto
        {
            TodayShift = todayShift.Adapt<ShiftInfoDto>(),
            NextShift = nextShift.Adapt<ShiftInfoDto>(),
            CurrentAttendance = currentAttendance,
            AttendanceStats = new ()
        };

        return AppResponse<EmployeeDashboardDto>.Success(dashboardData);
    }

    
}
