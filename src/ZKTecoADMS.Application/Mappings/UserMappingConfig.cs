using ZKTecoADMS.Application.Commands.Users.CreateUser;
using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.DeviceName, src => src.Device.DeviceName)
            .Map(dest => dest.Pin, src => src.Pin)
            .Map(dest => dest, src => src)
            .Map(dest => dest.ApplicationUser, src => src.ApplicationUser != null
                ? new UserAccountDto
                {
                    Email = src.ApplicationUser.Email!,
                    FirstName = src.ApplicationUser.FirstName!,
                    LastName = src.ApplicationUser.LastName!,
                    PhoneNumber = src.ApplicationUser.PhoneNumber,
                }
                : null);

        // Explicit mapping for CreateUserRequest to CreateUserCommand
        config.NewConfig<CreateUserRequest, CreateUserCommand>()
            .Map(dest => dest.Pin, src => src.Pin)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CardNumber, src => src.CardNumber)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.Privilege, src => src.Privilege)
            .Map(dest => dest.Department, src => src.Department)
            .Map(dest => dest.DeviceIds, src => src.DeviceIds);
    }
}