using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost;

public class CDataPostHandler(
    IDeviceService deviceService,
    ILogger<CDataPostHandler> logger,
    IServiceProvider serviceProvider
    ) : ICommandHandler<CDataPostCommand, string>
{
    public async Task<string> Handle(CDataPostCommand request, CancellationToken cancellationToken)
    {
        var sn = request.SN;
        if (string.IsNullOrWhiteSpace(request.Body))
        {
            logger.LogWarning("Empty body received from device {SerialNumber}", sn);
            
            return ClockResponses.Fail;
        }

        var device = await deviceService.GetDeviceBySerialNumberAsync(sn);
        if (device == null)
        {
            logger.LogError("Device not found: {SerialNumber}", sn);

            return ClockResponses.Fail;
        }

        var strategyContext = new PostStrategyContext(serviceProvider, request.Table.ToUpper());
        await strategyContext.ExecuteAsync(device, request.Body);

        return ClockResponses.Ok;
    }
}