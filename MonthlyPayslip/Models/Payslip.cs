namespace MonthlyPayslip.Models
{
    public class Payslip
    {
        public string EmployeeName { get; set; }
        public int GrossMonthlyIncome { get; set; }
        public int MonthlyIncomeTax { get; set; }
        public int NetMonthlyIncome { get; set; }
    }
}
