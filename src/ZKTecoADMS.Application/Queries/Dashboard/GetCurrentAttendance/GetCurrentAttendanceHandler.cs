using MediatR;
using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Dashboard.GetTodayShift;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Dashboard.GetCurrentAttendance;

public class GetCurrentAttendanceHandler(
    IRepository<Attendance> attendanceRepository,
    UserManager<ApplicationUser> userRepository,
    IMediator mediator
) : IQueryHandler<GetCurrentAttendanceQuery, AppResponse<AttendanceInfoDto>>
{
    public async Task<AppResponse<AttendanceInfoDto>> Handle(
        GetCurrentAttendanceQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId.ToString());
        if (user?.Employee == null)
        {
            return AppResponse<AttendanceInfoDto>.Success(null);
        }

        var today = DateTime.Now.Date;
        var attendances = await attendanceRepository.GetAllAsync(cancellationToken: cancellationToken);
        
        var todayAttendances = attendances
            .Where(a => a.PIN == user.Employee.Pin)
            .Where(a => a.AttendanceTime.Date == today)
            .OrderBy(a => a.AttendanceTime)
            .ToList();

        if (!todayAttendances.Any())
        {
            return AppResponse<AttendanceInfoDto>.Success(null);
        }

        var checkIn = todayAttendances.FirstOrDefault(a => a.AttendanceState == AttendanceStates.CheckIn);
        var checkOut = todayAttendances.LastOrDefault(a => a.AttendanceState == AttendanceStates.CheckOut);

        var checkInTime = checkIn?.AttendanceTime;
        var checkOutTime = checkOut?.AttendanceTime;

        string status = "not-started";
        if (checkInTime.HasValue && !checkOutTime.HasValue)
            status = "checked-in";
        else if (checkInTime.HasValue && checkOutTime.HasValue)
            status = "checked-out";

        double workHours = 0;
        if (checkInTime.HasValue && checkOutTime.HasValue)
        {
            workHours = (checkOutTime.Value - checkInTime.Value).TotalHours;
        }
        else if (checkInTime.HasValue)
        {
            workHours = (DateTime.Now - checkInTime.Value).TotalHours;
        }

        bool isLate = false;
        int? lateMinutes = null;
        bool isEarlyOut = false;
        int? earlyOutMinutes = null;

        // Get today's shift to compare
        var todayShiftResult = await mediator.Send(new GetTodayShiftQuery(request.UserId), cancellationToken);
        if (todayShiftResult.IsSuccess && todayShiftResult.Data != null && checkInTime.HasValue)
        {
            var todayShift = todayShiftResult.Data!;
            var expectedStartTime = todayShift.StartTime;
            
            if (checkInTime.Value > expectedStartTime)
            {
                isLate = true;
                lateMinutes = (int)(checkInTime.Value - expectedStartTime).TotalMinutes;
            }

            if (checkOutTime.HasValue)
            {
                var expectedEndTime = todayShift.EndTime;
                if (checkOutTime.Value < expectedEndTime)
                {
                    isEarlyOut = true;
                    earlyOutMinutes = (int)(expectedEndTime - checkOutTime.Value).TotalMinutes;
                }
            }
        }

        var dto = new AttendanceInfoDto
        {
            Id = checkIn?.Id ?? Guid.NewGuid(),
            CheckInTime = checkInTime,
            CheckOutTime = checkOutTime,
            WorkHours = Math.Round(workHours, 2),
            Status = status,
            IsLate = isLate,
            IsEarlyOut = isEarlyOut,
            LateMinutes = lateMinutes,
            EarlyOutMinutes = earlyOutMinutes
        };

        return AppResponse<AttendanceInfoDto>.Success(dto);
    }
}
