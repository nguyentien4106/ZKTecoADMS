using ZKTecoADMS.Application.Behaviours;

namespace ZKTecoADMS.Application.Queries.IClock.CDataGet;

public record CDataGetQuery(string SN, string? Options, string? PushVer, string? Language) : IQuery<string>, IIClockRequest;