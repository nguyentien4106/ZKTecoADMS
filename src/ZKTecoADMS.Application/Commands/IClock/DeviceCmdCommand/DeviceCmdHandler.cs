using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Extensions;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand;

public class DeviceCmdHandler(
    IDeviceCmdService deviceCmdService,
    IDeviceCommandStrategyFactory strategyFactory,
    IDeviceService deviceService,
    ILogger<DeviceCmdHandler> logger
    ) : ICommandHandler<DeviceCmdCommand, string>
{
    public async Task<string> Handle(DeviceCmdCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceService.GetDeviceBySerialNumberAsync(request.SN);
        if (device == null)
        {
            logger.LogWarning("Received DeviceCmd for unknown device SN: {SN}", request.SN);
            return ClockResponses.Fail;
        }
        
        var response = request.Body.ParseClockResponse();
        
        await deviceCmdService.UpdateCommandAfterExecutedAsync(response);
        var (commandType, objectRefId) = await deviceCmdService.GetCommandTypesAndIdAsync(response.CommandId);

        var strategy = strategyFactory.GetStrategy(commandType);
        if (strategy != null)
        {
            await strategy.ExecuteAsync(device, objectRefId, response, cancellationToken);
        }
        
        return ClockResponses.Ok;
    }
}