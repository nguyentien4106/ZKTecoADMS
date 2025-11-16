using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.IClock.CDataGet;

public class CDataGetHandler(IDeviceService deviceService) : IQueryHandler<CDataGetQuery, string>
{
    public async Task<string> Handle(CDataGetQuery request, CancellationToken cancellationToken)
    {
        var sn = request.SN;
        
        var device = await deviceService.GetDeviceBySerialNumberAsync(sn);

        if (device is not { IsActive: true })
        {
            return ClockResponses.Fail;
        }
            
        await deviceService.UpdateDeviceHeartbeatAsync(sn);

        var response = $"GET OPTION FROM: {sn}\r\n" +
                       "ATTLOGStamp=9999\r\n" + 
                       "OPERLOGStamp=9999\r\n" + 
                       "ErrorDelay=30\r\n" +
                       "Delay=5\r\n" +
                       "TransTimes=0\r\n" +
                       "TransInterval=0\r\n" +
                       "Delay=5\r\n" +
                       "TransFlag=1111111100\r\n" +
                       "Realtime=1\r\n" +
                       "TimeZone=+7:00\r\n" +
                       "Timeout=20\r\n" +
                       "ServerVer=2.0.4\r\n" +
                       "Encrypt=0";

        return response;
    }
}