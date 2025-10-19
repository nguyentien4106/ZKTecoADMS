using System.Text;
using System.Text.Json;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.Extensions;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.ClockCmdResponse;

public class ResultCommandHandler(IDeviceService deviceService) : ICommandHandler<ResultCommand, string>
{
    public async Task<string> Handle(ResultCommand request, CancellationToken cancellationToken)
    {
        var SN = request.SN;
        
        using var reader = new StreamReader(request.Body, Encoding.UTF8);
        var body = await reader.ReadToEndAsync(cancellationToken);
        var response = body.ParseClockResponse();
    
        await deviceService.UpdateCommandStatusAsync(response.ID, CommandStatus.Sent, response.CMD, response.Message);
        
        return ClockResponses.Ok;
    }
}