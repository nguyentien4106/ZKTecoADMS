using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Dashboard.GetAttendanceStats;

public class GetAttendanceStatsHandler(
    IRepository<Shift> shiftRepository,
    IRepository<Attendance> attendanceRepository,
    UserManager<ApplicationUser> userRepository
) : IQueryHandler<GetAttendanceStatsQuery, AppResponse<AttendanceStatsDto>>
{
    public async Task<AppResponse<AttendanceStatsDto>> Handle(
        GetAttendanceStatsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId.ToString());
        if (user?.Employee == null)
        {
            return AppResponse<AttendanceStatsDto>.Success(new AttendanceStatsDto { Period = request.Period });
        }

        var (startDate, endDate) = GetDateRange(request.Period);
        
        var shifts = await shiftRepository.GetAllAsync(cancellationToken: cancellationToken);
        var attendances = await attendanceRepository.GetAllAsync(cancellationToken: cancellationToken);

        var workShifts = shifts
            .Where(s => s.EmployeeUserId == user.Id)
            .Where(s => s.Status == ShiftStatus.Approved)
            .Where(s => s.StartTime.Date >= startDate && s.StartTime.Date <= endDate)
            .OrderBy(s => s.StartTime)
            .ToList();

        var totalWorkDays = workShifts.Count;
        if (totalWorkDays == 0)
        {
            return AppResponse<AttendanceStatsDto>.Success(new AttendanceStatsDto
            {
                Period = request.Period,
                TotalWorkDays = 0
            });
        }

        var userAttendances = attendances
            .Where(a => a.PIN == user.Employee.Pin)
            .Where(a => a.AttendanceTime.Date >= startDate && a.AttendanceTime.Date <= endDate)
            .OrderBy(a => a.AttendanceTime)
            .ToList();

        var attendanceByDate = userAttendances
            .GroupBy(a => a.AttendanceTime.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        int presentDays = 0;
        int lateCheckIns = 0;
        int earlyCheckOuts = 0;
        double totalWorkHours = 0;

        foreach (var shift in workShifts)
        {
            var shiftDate = shift.StartTime.Date;
            
            if (attendanceByDate.TryGetValue(shiftDate, out var dayAttendances))
            {
                var checkIn = dayAttendances.FirstOrDefault(a => a.AttendanceState == AttendanceStates.CheckIn);
                var checkOut = dayAttendances.LastOrDefault(a => a.AttendanceState == AttendanceStates.CheckOut);

                if (checkIn != null)
                {
                    presentDays++;

                    if (checkIn.AttendanceTime > shift.StartTime)
                    {
                        lateCheckIns++;
                    }

                    if (checkOut != null && checkOut.AttendanceTime < shift.EndTime)
                    {
                        earlyCheckOuts++;
                    }

                    if (checkOut != null)
                    {
                        totalWorkHours += (checkOut.AttendanceTime - checkIn.AttendanceTime).TotalHours;
                    }
                }
            }
        }

        int absentDays = totalWorkDays - presentDays;
        double attendanceRate = totalWorkDays > 0 ? (presentDays * 100.0 / totalWorkDays) : 0;
        double punctualityRate = presentDays > 0 ? ((presentDays - lateCheckIns) * 100.0 / presentDays) : 100;
        string avgWorkHours = presentDays > 0 ? (totalWorkHours / presentDays).ToString("F1") : "0.0";

        var dto = new AttendanceStatsDto
        {
            TotalWorkDays = totalWorkDays,
            PresentDays = presentDays,
            AbsentDays = absentDays,
            LateCheckIns = lateCheckIns,
            EarlyCheckOuts = earlyCheckOuts,
            AttendanceRate = Math.Round(attendanceRate, 2),
            PunctualityRate = Math.Round(punctualityRate, 2),
            AverageWorkHours = avgWorkHours,
            Period = request.Period
        };

        return AppResponse<AttendanceStatsDto>.Success(dto);
    }

    private static (DateTime startDate, DateTime endDate) GetDateRange(string period)
    {
        var now = DateTime.UtcNow;
        var endDate = now.Date;

        return period.ToLower() switch
        {
            "week" => (now.AddDays(-7).Date, endDate),
            "year" => (now.AddYears(-1).Date, endDate),
            _ => (now.AddMonths(-1).Date, endDate)
        };
    }
}
