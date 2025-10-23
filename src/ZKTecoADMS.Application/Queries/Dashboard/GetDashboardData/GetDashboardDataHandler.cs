using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;
using ZKTecoADMS.Infrastructure.Repositories;

namespace ZKTecoADMS.Application.Queries.Dashboard.GetDashboardData;

public class GetDashboardDataHandler(
    IUserRepository userRepository,
    IDeviceRepository deviceRepository,
    IAttendanceRepository attendanceRepository
) : IQueryHandler<GetDashboardDataQuery, AppResponse<DashboardDataDto>>
{
    private const int StandardWorkStartHour = 9; // 9 AM
    private const int LateThresholdMinutes = 15;

    public async Task<AppResponse<DashboardDataDto>> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var todayStart = today.Date;
        var todayEnd = today.Date.AddDays(1).AddTicks(-1);

        // Get devices first (filtered by user) - DbContext cannot handle parallel operations
        var allDevices = (await deviceRepository.GetAllAsync(
            filter: d => d.ApplicationUserId == request.UserId,
            cancellationToken: cancellationToken
        )).ToList();
        
        // If user has no devices, return empty dashboard
        if (!allDevices.Any())
        {
            return AppResponse<DashboardDataDto>.Success(new DashboardDataDto
            {
                Summary = new DashboardSummaryDto(),
                TopPerformers = new List<EmployeePerformanceDto>(),
                LateEmployees = new List<EmployeePerformanceDto>(),
                DepartmentStats = new List<DepartmentStatisticsDto>(),
                AttendanceTrends = new List<AttendanceTrendDto>(),
                DeviceStatuses = new List<DeviceStatusDto>()
            });
        }

        var userDeviceIds = allDevices.Select(d => d.Id).ToHashSet();
        
        // Get users filtered by devices belonging to current user
        var allUsers = (await userRepository.GetAllAsync(
            filter: u => userDeviceIds.Contains(u.DeviceId),
            cancellationToken: cancellationToken
        )).ToList();
        
        // Get attendances filtered by devices belonging to current user
        var allAttendances = (await attendanceRepository.GetAllAsync(
            filter: a => userDeviceIds.Contains(a.DeviceId),
            cancellationToken: cancellationToken
        )).ToList();

        // Filter attendances by date range
        var filteredAttendances = allAttendances
            .Where(a => a.AttendanceTime >= request.StartDate && a.AttendanceTime <= request.EndDate)
            .ToList();

        var todayAttendances = allAttendances
            .Where(a => a.AttendanceTime >= todayStart && a.AttendanceTime <= todayEnd)
            .ToList();

        // Filter users by department if specified
        var users = string.IsNullOrEmpty(request.Department)
            ? allUsers
            : allUsers.Where(u => u.Department == request.Department).ToList();

        // Build dashboard data
        var dashboardData = new DashboardDataDto
        {
            Summary = BuildSummary(users, allDevices, todayAttendances, allUsers),
            TopPerformers = BuildTopPerformers(users, filteredAttendances, request.TopPerformersCount, request.StartDate, request.EndDate),
            LateEmployees = BuildLateEmployees(users, filteredAttendances, request.LateEmployeesCount, request.StartDate, request.EndDate),
            DepartmentStats = BuildDepartmentStatistics(allUsers, todayAttendances, filteredAttendances, request.StartDate, request.EndDate),
            AttendanceTrends = BuildAttendanceTrends(allUsers, allAttendances, request.StartDate, request.EndDate, request.TrendDays),
            DeviceStatuses = BuildDeviceStatuses(allDevices, todayAttendances, allUsers)
        };

        return AppResponse<DashboardDataDto>.Success(dashboardData);
    }

    private DashboardSummaryDto BuildSummary(
        List<User> users,
        List<Device> devices,
        List<Attendance> todayAttendances,
        List<User> allUsers)
    {
        var todayCheckIns = todayAttendances.Count(a => a.AttendanceState == AttendanceStates.CheckIn);
        var todayCheckOuts = todayAttendances.Count(a => a.AttendanceState == AttendanceStates.CheckOut);
        
        var usersCheckedInToday = todayAttendances
            .Where(a => a.AttendanceState == AttendanceStates.CheckIn)
            .Select(a => a.UserId)
            .Distinct()
            .Count();

        var todayAbsences = users.Count(u => u.IsActive) - usersCheckedInToday;

        var todayLateArrivals = todayAttendances
            .Where(a => a.AttendanceState == AttendanceStates.CheckIn)
            .Count(a => IsLateArrival(a.AttendanceTime));

        var averageAttendanceRate = users.Any()
            ? (double)usersCheckedInToday / users.Count(u => u.IsActive) * 100
            : 0;

        return new DashboardSummaryDto
        {
            TotalEmployees = users.Count,
            ActiveEmployees = users.Count(u => u.IsActive),
            InactiveEmployees = users.Count(u => !u.IsActive),
            TotalDevices = devices.Count,
            OnlineDevices = devices.Count(d => d.DeviceStatus == "Online"),
            OfflineDevices = devices.Count(d => d.DeviceStatus == "Offline"),
            TodayCheckIns = todayCheckIns,
            TodayCheckOuts = todayCheckOuts,
            TodayAbsences = Math.Max(0, todayAbsences),
            TodayLateArrivals = todayLateArrivals,
            AverageAttendanceRate = Math.Round(averageAttendanceRate, 2)
        };
    }

    private List<EmployeePerformanceDto> BuildTopPerformers(
        List<User> users,
        List<Attendance> attendances,
        int count,
        DateTime startDate,
        DateTime endDate)
    {
        var totalDays = (endDate - startDate).Days + 1;
        
        var performances = users.Where(u => u.IsActive).Select(user =>
        {
            var userAttendances = attendances.Where(a => a.UserId == user.Id).ToList();
            var checkIns = userAttendances.Where(a => a.AttendanceState == AttendanceStates.CheckIn).ToList();
            var checkOuts = userAttendances.Where(a => a.AttendanceState == AttendanceStates.CheckOut).ToList();

            var attendanceDays = checkIns.Select(a => a.AttendanceTime.Date).Distinct().Count();
            var onTimeDays = checkIns.Count(a => !IsLateArrival(a.AttendanceTime));
            var lateDays = checkIns.Count(a => IsLateArrival(a.AttendanceTime));
            var absentDays = totalDays - attendanceDays;

            var workHours = CalculateAverageWorkHours(checkIns, checkOuts);
            var averageLateTime = CalculateAverageLateTime(checkIns);

            var attendanceRate = totalDays > 0 ? (double)attendanceDays / totalDays * 100 : 0;
            var punctualityRate = attendanceDays > 0 ? (double)onTimeDays / attendanceDays * 100 : 0;

            return new EmployeePerformanceDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Department = user.Department ?? "N/A",
                TotalAttendanceDays = attendanceDays,
                OnTimeDays = onTimeDays,
                LateDays = lateDays,
                AbsentDays = absentDays,
                AttendanceRate = Math.Round(attendanceRate, 2),
                PunctualityRate = Math.Round(punctualityRate, 2),
                AverageWorkHours = workHours,
                AverageLateTime = averageLateTime,
                LastCheckIn = checkIns.OrderByDescending(a => a.AttendanceTime).FirstOrDefault()?.AttendanceTime,
                LastCheckOut = checkOuts.OrderByDescending(a => a.AttendanceTime).FirstOrDefault()?.AttendanceTime
            };
        }).ToList();

        return performances
            .OrderByDescending(p => p.AttendanceRate)
            .ThenByDescending(p => p.PunctualityRate)
            .Take(count)
            .ToList();
    }

    private List<EmployeePerformanceDto> BuildLateEmployees(
        List<User> users,
        List<Attendance> attendances,
        int count,
        DateTime startDate,
        DateTime endDate)
    {
        var totalDays = (endDate - startDate).Days + 1;
        
        var performances = users.Where(u => u.IsActive).Select(user =>
        {
            var userAttendances = attendances.Where(a => a.UserId == user.Id).ToList();
            var checkIns = userAttendances.Where(a => a.AttendanceState == AttendanceStates.CheckIn).ToList();
            var checkOuts = userAttendances.Where(a => a.AttendanceState == AttendanceStates.CheckOut).ToList();

            var attendanceDays = checkIns.Select(a => a.AttendanceTime.Date).Distinct().Count();
            var onTimeDays = checkIns.Count(a => !IsLateArrival(a.AttendanceTime));
            var lateDays = checkIns.Count(a => IsLateArrival(a.AttendanceTime));
            var absentDays = totalDays - attendanceDays;

            var workHours = CalculateAverageWorkHours(checkIns, checkOuts);
            var averageLateTime = CalculateAverageLateTime(checkIns);

            var attendanceRate = totalDays > 0 ? (double)attendanceDays / totalDays * 100 : 0;
            var punctualityRate = attendanceDays > 0 ? (double)onTimeDays / attendanceDays * 100 : 0;

            return new EmployeePerformanceDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Department = user.Department ?? "N/A",
                TotalAttendanceDays = attendanceDays,
                OnTimeDays = onTimeDays,
                LateDays = lateDays,
                AbsentDays = absentDays,
                AttendanceRate = Math.Round(attendanceRate, 2),
                PunctualityRate = Math.Round(punctualityRate, 2),
                AverageWorkHours = workHours,
                AverageLateTime = averageLateTime,
                LastCheckIn = checkIns.OrderByDescending(a => a.AttendanceTime).FirstOrDefault()?.AttendanceTime,
                LastCheckOut = checkOuts.OrderByDescending(a => a.AttendanceTime).FirstOrDefault()?.AttendanceTime
            };
        }).Where(p => p.LateDays > 0).ToList();

        return performances
            .OrderByDescending(p => p.LateDays)
            .ThenBy(p => p.PunctualityRate)
            .Take(count)
            .ToList();
    }

    private List<DepartmentStatisticsDto> BuildDepartmentStatistics(
        List<User> allUsers,
        List<Attendance> todayAttendances,
        List<Attendance> periodAttendances,
        DateTime startDate,
        DateTime endDate)
    {
        var departments = allUsers
            .Where(u => !string.IsNullOrEmpty(u.Department))
            .GroupBy(u => u.Department)
            .Select(g => g.Key!)
            .ToList();

        return departments.Select(dept =>
        {
            var deptUsers = allUsers.Where(u => u.Department == dept && u.IsActive).ToList();
            var deptUserIds = deptUsers.Select(u => u.Id).ToHashSet();

            var todayDeptAttendances = todayAttendances.Where(a => a.UserId.HasValue && deptUserIds.Contains(a.UserId.Value)).ToList();
            var periodDeptAttendances = periodAttendances.Where(a => a.UserId.HasValue && deptUserIds.Contains(a.UserId.Value)).ToList();

            var activeToday = todayDeptAttendances
                .Where(a => a.AttendanceState == AttendanceStates.CheckIn)
                .Select(a => a.UserId)
                .Distinct()
                .Count();

            var absentToday = deptUsers.Count - activeToday;
            
            var lateToday = todayDeptAttendances
                .Where(a => a.AttendanceState == AttendanceStates.CheckIn)
                .Count(a => IsLateArrival(a.AttendanceTime));

            var totalDays = (endDate - startDate).Days + 1;
            var totalPossibleAttendances = deptUsers.Count * totalDays;
            
            var checkIns = periodDeptAttendances.Where(a => a.AttendanceState == AttendanceStates.CheckIn).ToList();
            var checkOuts = periodDeptAttendances.Where(a => a.AttendanceState == AttendanceStates.CheckOut).ToList();
            
            var actualAttendances = checkIns.Select(a => new { a.UserId, Date = a.AttendanceTime.Date }).Distinct().Count();
            var attendanceRate = totalPossibleAttendances > 0 ? (double)actualAttendances / totalPossibleAttendances * 100 : 0;
            
            var onTimeCount = checkIns.Count(a => !IsLateArrival(a.AttendanceTime));
            var punctualityRate = checkIns.Count > 0 ? (double)onTimeCount / checkIns.Count * 100 : 0;

            var avgWorkHours = CalculateAverageWorkHours(checkIns, checkOuts);

            return new DepartmentStatisticsDto
            {
                Department = dept,
                TotalEmployees = deptUsers.Count,
                ActiveToday = activeToday,
                AbsentToday = Math.Max(0, absentToday),
                LateToday = lateToday,
                AttendanceRate = Math.Round(attendanceRate, 2),
                PunctualityRate = Math.Round(punctualityRate, 2),
                AverageWorkHours = avgWorkHours
            };
        }).OrderByDescending(d => d.AttendanceRate).ToList();
    }

    private List<AttendanceTrendDto> BuildAttendanceTrends(
        List<User> users,
        List<Attendance> allAttendances,
        DateTime startDate,
        DateTime endDate,
        int trendDays)
    {
        var actualStartDate = endDate.AddDays(-trendDays + 1);
        if (actualStartDate < startDate)
            actualStartDate = startDate;

        var trends = new List<AttendanceTrendDto>();
        var activeUsers = users.Count(u => u.IsActive);

        for (var date = actualStartDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var dayStart = date;
            var dayEnd = date.AddDays(1).AddTicks(-1);
            
            var dayAttendances = allAttendances
                .Where(a => a.AttendanceTime >= dayStart && a.AttendanceTime <= dayEnd)
                .ToList();

            var checkIns = dayAttendances.Count(a => a.AttendanceState == AttendanceStates.CheckIn);
            var checkOuts = dayAttendances.Count(a => a.AttendanceState == AttendanceStates.CheckOut);
            
            var uniqueCheckIns = dayAttendances
                .Where(a => a.AttendanceState == AttendanceStates.CheckIn)
                .Select(a => a.UserId)
                .Distinct()
                .Count();

            var lateArrivals = dayAttendances
                .Where(a => a.AttendanceState == AttendanceStates.CheckIn)
                .Count(a => IsLateArrival(a.AttendanceTime));

            var absences = activeUsers - uniqueCheckIns;
            var attendanceRate = activeUsers > 0 ? (double)uniqueCheckIns / activeUsers * 100 : 0;

            trends.Add(new AttendanceTrendDto
            {
                Date = date,
                TotalCheckIns = checkIns,
                TotalCheckOuts = checkOuts,
                LateArrivals = lateArrivals,
                Absences = Math.Max(0, absences),
                AttendanceRate = Math.Round(attendanceRate, 2)
            });
        }

        return trends.OrderBy(t => t.Date).ToList();
    }

    private List<DeviceStatusDto> BuildDeviceStatuses(
        List<Device> devices,
        List<Attendance> todayAttendances,
        List<User> allUsers)
    {
        return devices.Select(device =>
        {
            var deviceUsers = allUsers.Count(u => u.DeviceId == device.Id);
            var todayDeviceAttendances = todayAttendances.Count(a => a.DeviceId == device.Id);

            return new DeviceStatusDto
            {
                DeviceId = device.Id,
                DeviceName = device.DeviceName,
                Location = device.Location ?? "N/A",
                Status = device.DeviceStatus,
                LastOnline = device.LastOnline,
                RegisteredUsers = deviceUsers,
                TodayAttendances = todayDeviceAttendances
            };
        }).OrderBy(d => d.Status).ThenBy(d => d.DeviceName).ToList();
    }

    private bool IsLateArrival(DateTime checkInTime)
    {
        var standardStart = new DateTime(
            checkInTime.Year,
            checkInTime.Month,
            checkInTime.Day,
            StandardWorkStartHour,
            LateThresholdMinutes,
            0);

        return checkInTime > standardStart;
    }

    private TimeSpan CalculateAverageWorkHours(List<Attendance> checkIns, List<Attendance> checkOuts)
    {
        if (!checkIns.Any() || !checkOuts.Any())
            return TimeSpan.Zero;

        var workDays = checkIns
            .GroupBy(ci => ci.AttendanceTime.Date)
            .Select(g =>
            {
                var dayCheckIn = g.OrderBy(a => a.AttendanceTime).First();
                var dayCheckOut = checkOuts
                    .Where(co => co.UserId == dayCheckIn.UserId && co.AttendanceTime.Date == dayCheckIn.AttendanceTime.Date)
                    .OrderByDescending(a => a.AttendanceTime)
                    .FirstOrDefault();

                if (dayCheckOut != null)
                {
                    return dayCheckOut.AttendanceTime - dayCheckIn.AttendanceTime;
                }
                return TimeSpan.Zero;
            })
            .Where(ts => ts > TimeSpan.Zero)
            .ToList();

        if (!workDays.Any())
            return TimeSpan.Zero;

        var averageTicks = (long)workDays.Average(ts => ts.Ticks);
        return TimeSpan.FromTicks(averageTicks);
    }

    private TimeSpan? CalculateAverageLateTime(List<Attendance> checkIns)
    {
        var lateTimes = checkIns
            .Where(a => IsLateArrival(a.AttendanceTime))
            .Select(a =>
            {
                var standardStart = new DateTime(
                    a.AttendanceTime.Year,
                    a.AttendanceTime.Month,
                    a.AttendanceTime.Day,
                    StandardWorkStartHour,
                    0,
                    0);
                return a.AttendanceTime - standardStart;
            })
            .ToList();

        if (!lateTimes.Any())
            return null;

        var averageTicks = (long)lateTimes.Average(ts => ts.Ticks);
        return TimeSpan.FromTicks(averageTicks);
    }
}
