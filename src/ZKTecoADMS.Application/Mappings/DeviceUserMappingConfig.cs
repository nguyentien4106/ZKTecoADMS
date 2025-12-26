using ZKTecoADMS.Application.DTOs.DeviceUsers;

namespace ZKTecoADMS.Application.Mappings;

public class DeviceUserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DeviceUser, DeviceUserDto>()
            .Map(dest => dest.DeviceName, src => src.Device.DeviceName)
            .Map(dest => dest.DeviceId, src => src.Device.Id)
            .Map(dest => dest, src => src)
           ;
       
    }
}