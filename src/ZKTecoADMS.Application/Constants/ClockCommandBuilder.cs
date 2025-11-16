namespace ZKTecoADMS.Application.Constants;

public static class ClockCommandBuilder
{
    public static string BuildAddOrUpdateUserCommand(User user)
    {
        return $"DATA UPDATE USERINFO PIN={user.Pin}\tName={user.Name}\tPri={user.Privilege}\tPasswd={user.Password}\tCard={user.CardNumber}\tGrp={user.GroupId}\tTZ=0000\tVerifyMode={user.VerifyMode}";
    }

    public static string BuildDeleteUserCommand(string pin)
    {
        return $"DATA DELETE USERINFO PIN={pin}";
    }

    public static string BuildGetAllUsersCommand()
    {
        return "DATA QUERY USERINFO";
    }

    /// <summary>
    /// Builds a command to query attendance logs within a time period.
    /// Default: Last 2 years up to today.
    /// Time format: YYYY-MM-DDThh:mm:ss
    /// </summary>
    public static string BuildGetAttendanceCommand(DateTime? startTime = null, DateTime? endTime = null)
    {
        var end = endTime ?? DateTime.Now;
        var start = startTime ?? end.AddYears(-2);

        // Format: YYYY-MM-DDThh:mm:ss (ISO 8601)
        var startTimeStr = start.ToString("yyyy-MM-ddTHH:mm:ss");
        var endTimeStr = end.ToString("yyyy-MM-ddTHH:mm:ss");

        return $"DATA QUERY ATTLOG StartTime={startTimeStr}\tEndTime={endTimeStr}";
    }
}