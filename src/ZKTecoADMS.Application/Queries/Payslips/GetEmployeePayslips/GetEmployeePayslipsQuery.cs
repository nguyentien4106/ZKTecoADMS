using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.DTOs.Payslips;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Application.Queries.Payslips.GetEmployeePayslips;

public record GetEmployeePayslipsQuery(
    Guid EmployeeUserId,
    bool IsManager
) : IQuery<AppResponse<List<PayslipDto>>>;
