namespace ZKTecoADMS.Application.DTOs.Attendances;

public class GetAttendancesByDeviceRequest
{
    public List<Guid> DeviceIds { get; set; } = new();
    
    public DateTime FromDate { get; set; }
    
    public DateTime ToDate { get; set; }
}