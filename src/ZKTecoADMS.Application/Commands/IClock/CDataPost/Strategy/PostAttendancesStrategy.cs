using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

public class PostAttendancesStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    private readonly IAttendanceRepository _attendanceRepository = serviceProvider.GetRequiredService<IAttendanceRepository>();
    private readonly ILogger<PostAttendancesStrategy> _logger = serviceProvider.GetRequiredService<ILogger<PostAttendancesStrategy>>();
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();

    // Field indices based on TFT protocol
    private const int PIN_INDEX = 0;
    private const int PUNCH_DATETIME_INDEX = 1;
    private const int ATTENDANCE_STATE_INDEX = 2;
    private const int VERIFY_MODE_INDEX = 3;
    private const int WORKCODE_INDEX = 4;
    private const int RESERVED_1_INDEX = 5;
    private const int RESERVED_2_INDEX = 6;
    private const int EXPECTED_FIELD_COUNT = 7;

    public async Task ProcessDataAsync(Device device, string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            _logger.LogWarning("Empty attendance data received from device {DeviceId}", device.Id);
            return;
        }

        // Split body by newlines to process multiple attendance records
        var lines = body.Split(new[] { '\n', '\r' });
        
        _logger.LogInformation("Processing {Count} attendance records from device {DeviceName}", 
            lines.Length, device.DeviceName);

        foreach (var line in lines)
        {
            try
            {
                await ProcessAttendanceLineAsync(device, line);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process attendance line: {Line}", line);
            }
        }
    }

    private async Task ProcessAttendanceLineAsync(Device device, string line)
    {
        // Format: [PIN]\t[Punch date/time]\t[Attendance State]\t[Verify Mode]\t[Workcode]\t[Reserved 1]\t[Reserved 2]
        var fields = line.Split('\t', StringSplitOptions.None);

        if (fields.Length < EXPECTED_FIELD_COUNT)
        {
            _logger.LogWarning(
                "Invalid attendance record format. Expected {Expected} fields but got {Actual}. Line: {Line}",
                EXPECTED_FIELD_COUNT, fields.Length, line);
            return;
        }

        var attendanceData = ParseAttendanceFields(fields);
        
        if (attendanceData == null)
        {
            _logger.LogWarning("Failed to parse attendance data from line: {Line}", line);
            return;
        }

        // Check if this attendance record already exists to avoid duplicates
        var exists = await _attendanceRepository.LogExistsAsync(
            device.Id, 
            attendanceData.PIN, 
            attendanceData.AttendanceTime);

        if (exists)
        {
            _logger.LogDebug("Duplicate attendance record skipped for PIN {PIN} at {Time}", 
                attendanceData.PIN, attendanceData.AttendanceTime);
            return;
        }

        // Create new attendance record
        var attendance = new Attendance
        {
            Id = Guid.NewGuid(),
            DeviceId = device.Id,
            PIN = attendanceData.PIN,
            AttendanceTime = attendanceData.AttendanceTime,
            AttendanceState = attendanceData.AttendanceState,
            VerifyMode = attendanceData.VerifyMode,
            WorkCode = attendanceData.WorkCode,
            UserId = (await _userRepository.GetUserByPinAsync(attendanceData.PIN))?.Id
        };

        await _attendanceRepository.AddAsync(attendance);
        
        _logger.LogInformation(
            "Attendance record saved: PIN={PIN}, Time={Time}, State={State}, VerifyMode={VerifyMode}",
            attendance.PIN, attendance.AttendanceTime, attendance.AttendanceState, attendance.VerifyMode);
    }

    private AttendanceData? ParseAttendanceFields(string[] fields)
    {
        try
        {
            var pin = fields[PIN_INDEX].Trim();
            
            // Parse datetime - format: yyyy-MM-dd HH:mm:ss
            if (!DateTime.TryParseExact(
                fields[PUNCH_DATETIME_INDEX].Trim(),
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var attendanceTime))
            {
                _logger.LogWarning("Failed to parse datetime: {DateTime}", fields[PUNCH_DATETIME_INDEX]);
                return null;
            }

            // Parse attendance state (integer to enum)
            if (!int.TryParse(fields[ATTENDANCE_STATE_INDEX].Trim(), out var stateValue))
            {
                _logger.LogWarning("Failed to parse attendance state: {State}", fields[ATTENDANCE_STATE_INDEX]);
                return null;
            }

            var attendanceState = MapAttendanceState(stateValue);
            var verifyMode = VerifyModes.Unknown;
            // Parse verify mode
            if (!int.TryParse(fields[VERIFY_MODE_INDEX].Trim(), out var verifyModeValue))
            {
                _logger.LogWarning("Failed to parse verify mode: {VerifyMode}", fields[VERIFY_MODE_INDEX]);
                verifyMode = MapVerifyMode(verifyModeValue);
            }

            // Parse workcode (optional)
            var workCode = fields.Length > WORKCODE_INDEX 
                ? fields[WORKCODE_INDEX].Trim() 
                : null;

            if (string.IsNullOrWhiteSpace(workCode))
            {
                workCode = null;
            }

            return new AttendanceData
            {
                PIN = pin,
                AttendanceTime = attendanceTime,
                AttendanceState = attendanceState,
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

    private static AttendanceStates MapAttendanceState(int stateValue)
    {
        // Map device state values to our enum
        // Based on common ZKTeco device states:
        // 0 = Check In, 1 = Check Out, 2 = Break Out, 3 = Break In, 4 = OT In, 5 = OT Out
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