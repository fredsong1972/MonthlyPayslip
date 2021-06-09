using System;
using System.Linq;
using MonthlyPayslip.Models;
using MonthlyPayslip.Repositories;
using Serilog;

namespace MonthlyPayslip.Services
{
    public class PayslipService : IPayslipService
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public PayslipService(ILogger logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public Payslip GenerateMonthlyPayslip(Employee employee)
        {
            var taxRates = _repository.GeTaxRates().ToList();
            var payslip = new Payslip
            {
                EmployeeName = employee.Name
            };

            try
            {
                double tax = 0;
                for (int i = 0; i < taxRates.Count; i++)
                {
                    if (employee.AnnualSalary >= taxRates[i].RangeBegin)
                    {
                        var rate = taxRates[i];
                        var income = employee.AnnualSalary > rate.RangeEnd
                            ? rate.RangeEnd
                            : employee.AnnualSalary;
                        tax += (double) (income - rate.RangeBegin + 1) * (double) rate.Rate / 12;
                        if (employee.AnnualSalary <= rate.RangeEnd)
                        {
                            break;
                        }

                    }
                }

                payslip.MonthlyIncomeTax = (int) Math.Round(tax);
                payslip.GrossMonthlyIncome = (int) Math.Round((double) employee.AnnualSalary / 12);
                payslip.NetMonthlyIncome = payslip.GrossMonthlyIncome - payslip.MonthlyIncomeTax;
                return payslip;

            }
            catch (Exception e)
            {
                _logger.Error(e, "Unhandled Exception");
                throw;
            }
        }
    }
}
