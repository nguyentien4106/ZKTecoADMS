using System.Text;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost;

public class CDataPostHandler(IDeviceService deviceService, ILogger<CDataPostHandler> logger) : ICommandHandler<CDataPostCommand, string>
{
    public async Task<string> Handle(CDataPostCommand request, CancellationToken cancellationToken)
    {
        var SN = request.SN;
        
        using var reader = new StreamReader(request.Body, Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(body))
        {
            logger.LogWarning("Empty body received from device {SerialNumber}", SN);
            
            return ClockResponses.Ok;
        }

        logger.LogDebug("Data received: {Data}", body);

        var device = await deviceService.GetDeviceBySerialNumberAsync(SN);
        if (device == null)
        {
            logger.LogError("Device not found: {SerialNumber}", SN);

            return ClockResponses.Fail;
        }

        var strategyContext = new PostStrategyContext(request.Table.ToUpper());
        await strategyContext.ExecuteAsync(device, body);

        // // Process based on table type
        // switch (request.Table?.ToUpper())
        // {
        //     case "ATTLOG":
        //         await ProcessAttendanceData(device, body);
        //         break;
        //     case "OPERLOG":
        //         await ProcessOperationLog(device, body);
        //         break;
        //     case "OPLOG":
        //         await ProcessOperationLog(device, body);
        //         break;
        //     case "USER":
        //         await ProcessUserData(device, body);
        //         break;
        //     default:
        //         logger.LogWarning("Unknown table type: {Table}", table);
        //         break;
        // }

        return ClockResponses.Ok;
    }
}