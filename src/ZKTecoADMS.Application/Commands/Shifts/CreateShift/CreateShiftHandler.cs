using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Shifts.CreateShift;

public class CreateShiftHandler(IRepository<Shift> shiftRepository) 
    : ICommandHandler<CreateShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate time range
            if (request.StartTime >= request.EndTime)
            {
                return AppResponse<ShiftDto>.Error("Start time must be before end time");
            }

            // Validate dates
            if (request.StartTime < DateTime.Now)
            {
                return AppResponse<ShiftDto>.Error("Cannot create shift for past dates");
            }

            // Check for overlapping shifts using optimized repository method
            var overlappingShift = await shiftRepository.GetSingleAsync(
                s => s.EmployeeUserId == request.EmployeeUserId &&
                    s.Status == ShiftStatus.Approved &&
                    s.Status == ShiftStatus.Pending &&
                    (
                        // New shift starts during an existing shift
                        (request.StartTime >= s.StartTime && request.StartTime < s.EndTime) ||
                        // New shift ends during an existing shift
                        (request.EndTime > s.StartTime && request.EndTime <= s.EndTime) ||
                        // New shift completely encompasses an existing shift
                        (request.StartTime <= s.StartTime && request.EndTime >= s.EndTime)
                    ),
                cancellationToken: cancellationToken
            );
            

            if (overlappingShift != null)
            {
                return AppResponse<ShiftDto>.Error(
                    $"This shift overlaps with an existing shift from {overlappingShift.StartTime:g} to {overlappingShift.EndTime:g}");
            }

            var shift = new Shift
            {
                EmployeeUserId = request.EmployeeUserId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Description = request.IsManager ? "Assigned by manager. " + request.Description : request.Description,
                Status = request.IsManager ? ShiftStatus.Approved : ShiftStatus.Pending,
                IsActive = true
            };

            var createdShift = await shiftRepository.AddAsync(shift, cancellationToken);
            var shiftDto = new ShiftDto
            {
                Id = createdShift.Id,
                EmployeeUserId = createdShift.EmployeeUserId,
                StartTime = createdShift.StartTime,
                EndTime = createdShift.EndTime,
                Description = createdShift.Description,
                Status = createdShift.Status,
                CreatedAt = createdShift.CreatedAt
            };
            
            return AppResponse<ShiftDto>.Success(shiftDto);
        }
        catch (ArgumentException ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
    }
}
