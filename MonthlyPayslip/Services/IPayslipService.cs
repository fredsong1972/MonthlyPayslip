using MonthlyPayslip.Models;

namespace MonthlyPayslip.Services
{
    public interface IPayslipService
    {
        Payslip GenerateMonthlyPayslip(Employee employee);
    }
}
