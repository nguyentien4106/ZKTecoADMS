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
        try
        {
            var (commandType, objectRefId) = await deviceCmdService.GetCommandTypesAndIdAsync(response.CommandId);

            var strategy = strategyFactory.GetStrategy(commandType);
            if (strategy != null)
            {
                logger.LogInformation("Processing DeviceCmd for device SN: {SN}, CommandId: {CommandId}, CommandType: {CommandType}", request.SN, response.CommandId, commandType.ToString());
                await strategy.ExecuteAsync(device, objectRefId, response, cancellationToken);
            }
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Error processing DeviceCmd for device SN: {SN}, CommandId: {CommandId}", request.SN, response.CommandId);
            return ClockResponses.Fail;
        }
        
        return ClockResponses.Ok;
    }
}