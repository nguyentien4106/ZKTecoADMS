using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.DTOs.Payslips;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Repositories;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Queries.Payslips.GetEmployeePayslips;

public class GetEmployeePayslipsHandler(
    IPayslipRepository payslipRepository
) : IQueryHandler<GetEmployeePayslipsQuery, AppResponse<List<PayslipDto>>>
{
    public async Task<AppResponse<List<PayslipDto>>> Handle(GetEmployeePayslipsQuery request, CancellationToken cancellationToken)
    {
        List<Payslip> payslips;
        if (request.IsManager)
        {
            payslips = await payslipRepository.GetPayslipsByManagerIdAsync(request.EmployeeUserId, DateTime.Now.Year, DateTime.Now.Month, cancellationToken);
        }
        else
        {
            payslips = await payslipRepository.GetByEmployeeUserIdAsync(request.EmployeeUserId, cancellationToken);
        }
        var dtos = payslips.Select(MapToDto).ToList();
        
        return AppResponse<List<PayslipDto>>.Success(dtos);
    }

    private PayslipDto MapToDto(Payslip payslip)
    {
        return new PayslipDto
        {
            Id = payslip.Id,
            EmployeeUserId = payslip.EmployeeUserId,
            EmployeeName = payslip.EmployeeUser?.UserName ?? string.Empty,
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
