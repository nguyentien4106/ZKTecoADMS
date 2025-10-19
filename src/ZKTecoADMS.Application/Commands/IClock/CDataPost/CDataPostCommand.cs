using ZKTecoADMS.Application.CQRS;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost;

public record CDataPostCommand(string SN, string Table, string Stamp, Stream Body) : ICommand<string>;