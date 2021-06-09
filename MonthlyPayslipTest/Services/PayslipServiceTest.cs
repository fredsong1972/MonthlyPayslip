using System.Collections.Generic;
using MonthlyPayslip.Models;
using MonthlyPayslip.Repositories;
using NSubstitute;
using MonthlyPayslip.Services;
using Serilog;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace MonthlyPayslipTest.Services
{
    public class PayslipServiceTest
    {
        private readonly IRepository _repository;
        private readonly PayslipService _subject;
        private readonly List<Employee> _employeeDetailsTestData;
        private List<TaxRate> _taxRateTestData;
        private readonly List<Payslip> _payslipExpectedResult;
        private List<Payslip> _testResult;

        public PayslipServiceTest()
        {
            _repository = Substitute.For<IRepository>();
            _subject = new PayslipService(Substitute.For<ILogger>(), _repository);

            _employeeDetailsTestData = new List<Employee>
            {
                new Employee
                {
                    Name = "Susan A",
                    AnnualSalary = 18000,
                },
                new Employee
                {
                    Name = "David A",
                    AnnualSalary = 60000,
                },
                new Employee
                {
                    Name = "Ryan B",
                    AnnualSalary = 120000,

                },
                new Employee
                {
                    Name = "Andrew B",
                    AnnualSalary = 240000,

                },
            };
            _taxRateTestData = new List<TaxRate>
            {
                new TaxRate {RangeBegin = 0, RangeEnd = 20000, Rate = 0},
                new TaxRate {RangeBegin = 20001, RangeEnd = 40000, Rate = (decimal) 0.10},
                new TaxRate {RangeBegin = 40001, RangeEnd = 80000, Rate = (decimal) 0.20},
                new TaxRate {RangeBegin = 80001, RangeEnd = 180000, Rate = (decimal) 0.30},
                new TaxRate {RangeBegin = 180001, RangeEnd = null, Rate = (decimal) 0.40},
            };

            _payslipExpectedResult = new List<Payslip>()
            {
                new Payslip
                {
                    EmployeeName = "Susan A", GrossMonthlyIncome =1500, MonthlyIncomeTax = 0, NetMonthlyIncome = 1500
                },
                new Payslip
                {
                    EmployeeName = "David A", GrossMonthlyIncome = 5000, MonthlyIncomeTax = 500, NetMonthlyIncome = 4500
                },
                new Payslip
                {
                    EmployeeName = "Ryan B", GrossMonthlyIncome = 10000, MonthlyIncomeTax = 1833, NetMonthlyIncome = 8167
                },
                new Payslip
                {
                    EmployeeName = "Andrew B", GrossMonthlyIncome = 20000, MonthlyIncomeTax = 5333, NetMonthlyIncome = 14667
                },
            };
        }

        #region Facts

        [Fact]
        public void GeneratePayslip()
        {
            this.Given(x => x.GivenGetTaxRatesReturnTaxRates())
                .When(x => WhenGeneratePayslipsIsCalled(_employeeDetailsTestData))
                .Then(x => ThenPayslipsShouldBeCorrect())
                .BDDfy();
        }

        #endregion

        #region Givens

        private void GivenGetTaxRatesReturnTaxRates()
        {
            _repository.GeTaxRates().ReturnsForAnyArgs(_taxRateTestData);
        }

        #endregion

        #region Whens

        private void WhenGeneratePayslipsIsCalled(List<Employee> employees)
        {
            _testResult = new List<Payslip>();
            foreach (var employee in employees)
            {
                _testResult.Add(_subject.GenerateMonthlyPayslip(employee));
            }
        }

        #endregion

        #region Thens

        private void ThenPayslipsShouldBeCorrect()
        {
            for (int i = 0; i < _testResult.Count; i++)
            {
                var result = _testResult[i];
                var expected = _payslipExpectedResult[i];

                result.EmployeeName.ShouldBe(expected.EmployeeName);
                result.GrossMonthlyIncome.ShouldBe(expected.GrossMonthlyIncome);
                result.MonthlyIncomeTax.ShouldBe(expected.MonthlyIncomeTax);
                result.NetMonthlyIncome.ShouldBe(expected.NetMonthlyIncome);
            }
        }

        #endregion

    }
}
