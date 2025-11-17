using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

public class PostAttendancesStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    private readonly IAttendanceService attendanceService = serviceProvider.GetRequiredService<IAttendanceService>();
    private readonly ILogger<PostAttendancesStrategy> _logger = serviceProvider.GetRequiredService<ILogger<PostAttendancesStrategy>>();
    private readonly IEmployeeService employeeService = serviceProvider.GetRequiredService<IEmployeeService>();
    private readonly IRepository<Attendance> attendanceRepository = serviceProvider.GetRequiredService<IRepository<Attendance>>();

    // Field indices based on TFT protocol
    // Format: [PIN]\t[Punch date/time]\t[Attendance State]\t[Verify Mode]\t[Workcode]\t[Reserved 1]\t[Reserved 2]
    private const int PIN_INDEX = 0;
    private const int PUNCH_DATETIME_INDEX = 1;
    private const int ATTENDANCE_STATE_INDEX = 2;
    private const int VERIFY_MODE_INDEX = 3;
    private const int WORKCODE_INDEX = 4;
    private const int RESERVED_1_INDEX = 5;
    private const int RESERVED_2_INDEX = 6;
    private const int MIN_FIELD_COUNT = 4; // Minimum required fields
    private const int EXPECTED_FIELD_COUNT = 7;
    private const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

    public async Task<string> ProcessDataAsync(Device device, string body)
    {
        var lines = SplitAttendanceLines(body);
        _logger.LogInformation("Processing {Count} attendance records from device {DeviceName}", 
            lines.Count, device.DeviceName);

        var attendances = await ProcessAttendanceLinesAsync(device, lines);
        
        if (attendances.Count == 0)
        {
            _logger.LogWarning("No valid attendance records to save from device {DeviceId}", device.Id);
            return ClockResponses.Ok;
        }

        await SaveAttendancesAsync(attendances, device.DeviceName);
        return ClockResponses.Ok;
    }

    private List<string> SplitAttendanceLines(string body)
    {
        return body.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
                   .Where(line => !string.IsNullOrWhiteSpace(line))
                   .ToList();
    }

    private async Task<List<Attendance>> ProcessAttendanceLinesAsync(Device device, List<string> lines)
    {
        var attendances = new List<Attendance>();

        foreach (var line in lines)
        {
            try
            {
                var attendance = await TryProcessAttendanceLineAsync(device, line);
                if (attendance != null)
                {
                    attendances.Add(attendance);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing attendance line from device {DeviceId}: {Line}",
                    device.Id, line);
            }
        }

        return attendances;
    }

    private async Task SaveAttendancesAsync(List<Attendance> attendances, string deviceName)
    {
        await attendanceRepository.AddRangeAsync(attendances);
        _logger.LogInformation("Successfully saved {Count} attendance records from device {DeviceName}",
            attendances.Count, deviceName);
    }

    private async Task<Attendance?> TryProcessAttendanceLineAsync(Device device, string line)
    {
        var fields = SplitLineIntoFields(line);
        
        if (!ValidateFieldCount(fields, line))
        {
            return null;
        }

        var attendanceData = ParseAttendanceFields(fields);
        if (attendanceData == null)
        {
            _logger.LogWarning("Failed to parse attendance data from line: {Line}", line);
            return null;
        }

        if (await IsDuplicateAttendanceAsync(device.Id, attendanceData))
        {
            _logger.LogDebug("Duplicate attendance record skipped for PIN {PIN} at {Time}", 
                attendanceData.PIN, attendanceData.AttendanceTime);
            return null;
        }

        return await CreateAttendanceRecordAsync(device.Id, attendanceData);
    }

    private string[] SplitLineIntoFields(string line)
    {
        return line.Split('\t', StringSplitOptions.None);
    }

    private bool ValidateFieldCount(string[] fields, string line)
    {
        if (fields.Length < MIN_FIELD_COUNT)
        {
            _logger.LogWarning(
                "Invalid attendance record format. Expected at least {Min} fields but got {Actual}. Line: {Line}",
                MIN_FIELD_COUNT, fields.Length, line);
            return false;
        }

        if (fields.Length < EXPECTED_FIELD_COUNT)
        {
            _logger.LogDebug(
                "Attendance record has fewer fields than expected. Expected {Expected} but got {Actual}",
                EXPECTED_FIELD_COUNT, fields.Length);
        }

        return true;
    }

    private async Task<bool> IsDuplicateAttendanceAsync(Guid deviceId, AttendanceData attendanceData)
    {
        return await attendanceService.LogExistsAsync(
            deviceId,
            attendanceData.PIN,
            attendanceData.AttendanceTime);
    }

    private async Task<Attendance> CreateAttendanceRecordAsync(Guid deviceId, AttendanceData attendanceData)
    {
        var employee = await employeeService.GetEmployeeByPinAsync(deviceId, attendanceData.PIN);

        return new Attendance
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            PIN = attendanceData.PIN,
            AttendanceTime = attendanceData.AttendanceTime,
            AttendanceState = attendanceData.AttendanceState,
            VerifyMode = attendanceData.VerifyMode,
            WorkCode = attendanceData.WorkCode,
            EmployeeId = employee?.Id
        };
    }

    private AttendanceData? ParseAttendanceFields(string[] fields)
    {
        try
        {
            var pin = ExtractPin(fields);
            var attendanceTime = ParseAttendanceTime(fields);
            var attendanceState = ParseAttendanceState(fields);
            var verifyMode = ParseVerifyMode(fields);
            var workCode = ExtractWorkCode(fields);

            if (attendanceTime == null || attendanceState == null)
            {
                return null;
            }

            return new AttendanceData
            {
                PIN = pin,
                AttendanceTime = attendanceTime.Value,
                AttendanceState = attendanceState.Value,
                VerifyMode = verifyMode,
                WorkCode = workCode
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing attendance fields");
            return null;
        }
    }

    private string ExtractPin(string[] fields)
    {
        return fields[PIN_INDEX].Trim();
    }

    private DateTime? ParseAttendanceTime(string[] fields)
    {
        var dateTimeString = fields[PUNCH_DATETIME_INDEX].Trim();
        
        if (DateTime.TryParseExact(
            dateTimeString,
            DATETIME_FORMAT,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var attendanceTime))
        {
            return attendanceTime;
        }

        _logger.LogWarning("Failed to parse datetime: {DateTime}. Expected format: {Format}", 
            dateTimeString, DATETIME_FORMAT);
        return null;
    }

    private AttendanceStates? ParseAttendanceState(string[] fields)
    {
        var stateString = fields[ATTENDANCE_STATE_INDEX].Trim();
        
        if (int.TryParse(stateString, out var stateValue))
        {
            return MapAttendanceState(stateValue);
        }

        _logger.LogWarning("Failed to parse attendance state: {State}", stateString);
        return null;
    }

    private VerifyModes ParseVerifyMode(string[] fields)
    {
        if (fields.Length <= VERIFY_MODE_INDEX)
        {
            return VerifyModes.Unknown;
        }

        var verifyModeString = fields[VERIFY_MODE_INDEX].Trim();
        
        if (int.TryParse(verifyModeString, out var verifyModeValue))
        {
            return MapVerifyMode(verifyModeValue);
        }

        _logger.LogDebug("Failed to parse verify mode: {VerifyMode}. Using Unknown", verifyModeString);
        return VerifyModes.Unknown;
    }

    private string? ExtractWorkCode(string[] fields)
    {
        if (fields.Length <= WORKCODE_INDEX)
        {
            return null;
        }

        var workCode = fields[WORKCODE_INDEX].Trim();
        return string.IsNullOrWhiteSpace(workCode) ? null : workCode;
    }

    /// <summary>
    /// Maps device attendance state values to our enum.
    /// Based on ZKTeco device protocol:
    /// 0=Check In, 1=Check Out, 2=Break Out, 3=Break In, 4=Meal Out, 5=Meal In
    /// </summary>
    private static AttendanceStates MapAttendanceState(int stateValue)
    {
        return stateValue switch
        {
            0 => AttendanceStates.CheckIn,
            1 => AttendanceStates.CheckOut,
            2 => AttendanceStates.BreakOut,
            3 => AttendanceStates.BreakIn,
            4 => AttendanceStates.MealOut,
            5 => AttendanceStates.MealIn,
            _ => AttendanceStates.CheckIn // Default to CheckIn for unknown states
        };
    }

    /// <summary>
    /// Maps device verify mode values to our enum.
    /// Based on ZKTeco device protocol verification methods.
    /// </summary>
    private static VerifyModes MapVerifyMode(int modeValue)
    {
        return modeValue switch
        {
            0 => VerifyModes.Password,
            1 => VerifyModes.Finger,
            2 => VerifyModes.Badge,
            3 => VerifyModes.PIN,
            4 => VerifyModes.PINAndFingerprint,
            5 => VerifyModes.FingerAndPassword,
            6 => VerifyModes.BadgeAndFinger,
            7 => VerifyModes.BadgeAndPassword,
            8 => VerifyModes.BadgeAndPasswordAndFinger,
            9 => VerifyModes.PINAndPasswordAndFinger,
            _ => VerifyModes.Unknown
        };
    }

    /// <summary>
    /// Internal data transfer object for parsed attendance data
    /// </summary>
    private class AttendanceData
    {
        public required string PIN { get; set; }
        public DateTime AttendanceTime { get; set; }
        public AttendanceStates AttendanceState { get; set; }
        public VerifyModes VerifyMode { get; set; }
        public string? WorkCode { get; set; }
    }
}