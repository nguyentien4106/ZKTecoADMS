using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Users.CreateUser;

public class CreateUserHandler(
    IDeviceService deviceService, 
    IRepository<User> userRepository
    ) : ICommandHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var users = request.DeviceIds.Select(deviceId => new User
            {
                PIN = request.PIN,
                FullName = request.FullName,
                CardNumber = request.CardNumber,
                Password = request.Password,
                GroupId = request.GroupId,
                Privilege = request.Privilege,
                VerifyMode = request.VerifyMode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Department = request.Department,
                IsActive = true,
                DeviceId = deviceId
            })
            .ToList();

        foreach (var user in users)
        {
            var result = await userRepository.AddAsync(user);
            
        }
        
        return users.FirstOrDefault();
    }
}