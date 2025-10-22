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
        var SN = request.SN;
        if (string.IsNullOrWhiteSpace(request.Body))
        {
            logger.LogWarning("Empty body received from device {SerialNumber}", SN);
            
            return ClockResponses.Fail;
        }

        var device = await deviceService.GetDeviceBySerialNumberAsync(SN);
        if (device == null)
        {
            logger.LogError("Device not found: {SerialNumber}", SN);

            return ClockResponses.Fail;
        }

        var strategyContext = new PostStrategyContext(serviceProvider, request.Table.ToUpper());
        await strategyContext.ExecuteAsync(device, request.Body);

        return ClockResponses.Ok;
    }
}