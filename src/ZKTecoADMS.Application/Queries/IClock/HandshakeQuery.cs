using ZKTecoADMS.Application.CQRS;

namespace ZKTecoADMS.Application.Queries.IClock;

public record HandshakeQuery(string SN, string? Options, string? PushVer, string? Language) : IQuery<string>;
