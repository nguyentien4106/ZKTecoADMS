namespace ZKTecoADMS.Application.DTOs.Devices;

public record DeviceCmdRequest(string Command, int Priority = 1);