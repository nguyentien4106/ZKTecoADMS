using ZKTecoADMS.Application.CQRS;

namespace ZKTecoADMS.Application.Commands.GetRequest;

public record GetRequestQuery(string SN) : ICommand<string>;