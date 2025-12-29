using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.DTOs.Payslips;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ZKTecoADMS.Application.Commands.Payslips.GeneratePayslip;

public class GeneratePayslipHandler(
    IPayslipRepository payslipRepository,
    IEmployeeSalaryProfileRepository employeeSalaryProfileRepository,
    IRepository<Shift> shiftRepository,
    UserManager<ApplicationUser> userManager
) : ICommandHandler<GeneratePayslipCommand, AppResponse<PayslipDto>>
{
    public async Task<AppResponse<PayslipDto>> Handle(GeneratePayslipCommand request, CancellationToken cancellationToken)
    {
        // Validate current month constraint
        var now = DateTime.Now;
        var requestedDate = new DateTime(request.Year, request.Month, 1);
        var currentMonthStart = new DateTime(now.Year, now.Month, 1);
        
        if (requestedDate < currentMonthStart || requestedDate > currentMonthStart)
        {
            return AppResponse<PayslipDto>.Fail("Payslips can only be generated for the current month");
        }

        // Check if user exists and get employee
        var user = await userManager.Users
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Id == request.EmployeeUserId, cancellationToken);
            
        if (user == null)
        {
            return AppResponse<PayslipDto>.Fail("Employee user not found");
        }

        var employee = user.Employee;

        if (employee == null)
        {
            return AppResponse<PayslipDto>.Fail("Employee not found");
        }

        // Check if employee has an active salary profile
        var activeSalaryProfile = await employeeSalaryProfileRepository.GetActiveByEmployeeIdAsync(
            employee.Id, 
            cancellationToken
        );
        
        if (activeSalaryProfile == null || !activeSalaryProfile.IsActive)
        {
            return AppResponse<PayslipDto>.Fail("Employee does not have an active salary profile");
        }

        // Load the salary profile
        var salaryProfileDetails = await employeeSalaryProfileRepository.GetByIdAsync(
            activeSalaryProfile.Id,
            cancellationToken: cancellationToken
        );

       

        // Check if payslip already exists for this period
        var existingPayslip = await payslipRepository.GetByEmployeeUserAndPeriodAsync(request.EmployeeUserId, request.Year, request.Month, cancellationToken);
        if (existingPayslip != null)
        {
            // delete current existing payslip to allow regeneration
            await payslipRepository.DeleteAsync(existingPayslip.Id, cancellationToken);
        }

        // Calculate period dates (start of month to current date)
        var periodStart = new DateTime(request.Year, request.Month, 1);
        var periodEnd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

        // Get approved shifts for the period
        var shifts = await shiftRepository.GetAllAsync(
            filter: s => 
                s.EmployeeUserId == request.EmployeeUserId &&
                s.Status == ShiftStatus.Approved &&
                s.CheckInAttendanceId != null &&
                s.CheckOutAttendanceId != null &&
                s.Leave == null &&
                s.StartTime >= periodStart &&
                s.EndTime <= periodEnd,
            cancellationToken: cancellationToken
        );

        var shiftsList = shifts?.ToList() ?? new List<Shift>();

        // Calculate work units based on salary rate type using shift times
        

        return AppResponse<PayslipDto>.Success(null);
    }

    private (decimal regular, decimal holiday, decimal nightShift) CalculateWorkUnits(
        List<Shift> shifts,
        SalaryRateType rateType)
    {
        decimal regular = 0;
        decimal holiday = 0;
        decimal nightShift = 0;

        foreach (var shift in shifts)
        {
            // Use shift start and end times
            var shiftStart = shift.StartTime;
            var shiftEnd = shift.EndTime;
            var duration = shiftEnd - shiftStart - TimeSpan.FromMinutes(shift.BreakTimeMinutes);

            // Calculate hours in shift
            var hoursInShift = (decimal)duration.TotalHours;

            // For hourly rate, count all hours
            if (rateType == SalaryRateType.Hourly)
            {
                regular += hoursInShift;

                // Check for night shift (example: 10 PM to 6 AM)
                if (shiftStart.Hour >= 22 || shiftStart.Hour < 6)
                {
                    nightShift += hoursInShift;
                }

                // Check for holiday shift (you can add holiday logic here based on shift.IsHoliday or date)
                // For now, we'll leave holiday calculation for future implementation
            }
            // For daily rate, count as days
            // For monthly, shifts are just tracked but salary is fixed
            else if (rateType == SalaryRateType.Monthly)
            {
                // Count shifts for record keeping
                regular += 1;
            }
        }

        return (regular, holiday, nightShift);
    }

    private decimal CalculateBaseSalary(decimal units, decimal rate, SalaryRateType rateType, int year, int month)
    {
        return rateType switch
        {
            SalaryRateType.Hourly => units * rate,
            SalaryRateType.Monthly => rate, // Full monthly salary
            _ => 0
        };
    }

    private decimal? CalculateHolidayPay(decimal? holidayUnits, decimal rate, decimal multiplier)
    {
        if (holidayUnits == null || holidayUnits == 0)
            return null;

        return holidayUnits.Value * rate * multiplier;
    }

    private decimal? CalculateNightShiftPay(decimal? nightShiftUnits, decimal rate, decimal multiplier)
    {
        if (nightShiftUnits == null || nightShiftUnits == 0)
            return null;

        return nightShiftUnits.Value * rate * multiplier;
    }

    private PayslipDto MapToDto(Payslip payslip)
    {
        return new PayslipDto
        {
            Id = payslip.Id,
            EmployeeUserId = payslip.EmployeeUserId,
            EmployeeName = payslip.EmployeeUser?.FirstName + " " + payslip.EmployeeUser?.LastName ?? string.Empty,
            SalaryProfileId = payslip.SalaryProfileId,
            SalaryProfileName = payslip.SalaryProfile?.Name ?? string.Empty,

            Year = payslip.Year,
            Month = payslip.Month,
            PeriodStart = payslip.PeriodStart,
            PeriodEnd = payslip.PeriodEnd,
            RegularWorkUnits = payslip.RegularWorkUnits,
            OvertimeUnits = payslip.OvertimeUnits,
            HolidayUnits = payslip.HolidayUnits,
            NightShiftUnits = payslip.NightShiftUnits,
            BaseSalary = payslip.BaseSalary,
            OvertimePay = payslip.OvertimePay,
            HolidayPay = payslip.HolidayPay,
            NightShiftPay = payslip.NightShiftPay,
            Bonus = payslip.Bonus,
            Deductions = payslip.Deductions,
            GrossSalary = payslip.GrossSalary,
            NetSalary = payslip.NetSalary,
            Currency = payslip.Currency,
            Status = payslip.Status,
            StatusName = payslip.Status.ToString(),
            GeneratedDate = payslip.GeneratedDate,
            GeneratedByUserName = payslip.GeneratedByUser?.UserName,
            ApprovedDate = payslip.ApprovedDate,
            ApprovedByUserName = payslip.ApprovedByUser?.UserName,
            PaidDate = payslip.PaidDate,
            Notes = payslip.Notes,
            CreatedAt = payslip.CreatedAt
        };
    }
}
