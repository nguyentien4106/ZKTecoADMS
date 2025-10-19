using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.IClock;

public class HandshakeHandler(IDeviceService deviceService) : IQueryHandler<HandshakeQuery, string>
{
    public async Task<string> Handle(HandshakeQuery request, CancellationToken cancellationToken)
    {
        var SN = request.SN;
        
        var device = await deviceService.GetDeviceBySerialNumberAsync(SN);

        if (device == null)
        {
            return ClockResponses.Fail;
        }
            
        await deviceService.UpdateDeviceHeartbeatAsync(SN);

        var response = $"GET OPTION FROM: {SN}\r\n" +
                       "ATTLOGStamp=9999\r\n" + 
                       "OPERLOGStamp=9999\r\n" + 
                       "ErrorDelay=60\r\n" +
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
        return "OK";
    }
}