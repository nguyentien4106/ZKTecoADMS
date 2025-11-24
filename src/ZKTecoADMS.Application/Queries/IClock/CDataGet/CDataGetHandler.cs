using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.IClock.CDataGet;

public class CDataGetHandler(
    IDeviceService deviceService,
    IRepository<Attendance> attendanceRepository
    ) : IQueryHandler<CDataGetQuery, string>
{
    public async Task<string> Handle(CDataGetQuery request, CancellationToken cancellationToken)
    {
        var sn = request.SN;
        
        var device = await deviceService.GetDeviceBySerialNumberAsync(sn);

        if (device is not { IsActive: true })
        {
            return ClockResponses.Fail;
        }

        if(request.type != null && request.type == "time")
        {
            return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+07:00");
        }
            
        await deviceService.UpdateDeviceHeartbeatAsync(sn);

        var lastAttendance = await attendanceRepository.GetLastOrDefaultAsync(
            keySelector: a => a.AttendanceTime,
            filter: a => a.DeviceId == device.Id,
            cancellationToken: cancellationToken);
        
        var ATTLOGStamp = lastAttendance?.AttendanceTime.ToString(DameTimeFormats.DeviceDateTimeFormat) ?? "0";

        var response = $"GET OPTION FROM: {sn}\r\n" +
                       $"ATTLOGStamp={ATTLOGStamp}\r\n" + 
                       "OPERLOGStamp=9999\r\n" + 
                       "ErrorDelay=30\r\n" +
                       "Delay=5\r\n" +
                       "TransTimes=0\r\n" +
                       "TransInterval=0\r\n" +
                       "TransFlag=1111111100\r\n" +
                       "Realtime=1\r\n" +
                       "TimeZone=+07:00\r\n" +
                       "Timeout=20\r\n" +
                       "SyncTime=5\r\n" +
                       "ServerVer=2.0.4\r\n" +
                       "Encrypt=0";

        return response;
    }
}