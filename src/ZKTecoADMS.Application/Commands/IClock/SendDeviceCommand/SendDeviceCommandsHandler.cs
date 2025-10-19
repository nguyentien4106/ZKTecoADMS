using System.Text;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.SendDeviceCommand;

public class SendDeviceCommandsHandler(IDeviceService deviceService) : ICommandHandler<SendDeviceCommands, string>
{
    public async Task<string> Handle(SendDeviceCommands request, CancellationToken cancellationToken)
    {
        var SN = request.SN;
        
        var device = await deviceService.GetDeviceBySerialNumberAsync(SN);
        if (device == null)
        {
            return ClockResponses.Ok;
        }

        var commands = await deviceService.GetPendingCommandsAsync(device.Id);
        
        if (commands.Any())
        {
            var response = new StringBuilder();
            foreach (var command in commands.OrderByDescending(c => c.Priority))
            {
                // Format: C:ID:CommandType:CommandData
                var cmd = $"C:{command.CommandId}:{command.Command}";
                response.AppendLine(cmd);
                    
                // Mark as sent
                await deviceService.MarkCommandAsSentAsync(command.Id);
            }

            return response.ToString();
        }
        
        return ClockResponses.Ok;
    }
}