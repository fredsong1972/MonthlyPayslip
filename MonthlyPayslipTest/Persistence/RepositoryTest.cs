using System;
using System.Collections.Generic;
using System.Linq;
using MonthlyPayslip.Models;
using MonthlyPayslip.Repositories;
using NSubstitute;
using Serilog;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PayslipAppTest.Persistence
{
    public class RepositoryTest
    {
        private IDataContext _appContext;
        private Repository _subject;
        private List<string[]> _testTaxRateData;
        private readonly List<TaxRate> _taxRateExpectedResult;
        private List<TaxRate> _taxRateTestResult;

        public RepositoryTest()
        {
            _testTaxRateData = new List<string[]>
            {
                new[] {"0", "20000", ""},
                new[] {"20001", "40000", "0.1"},
                new[] {"40001", "80000", "0.2"},
                new[] {"80001", "180000", "0.3"},
                new[] {"180001", "", "0.4"}
            };

            _taxRateExpectedResult = new List<TaxRate>
            {
                new TaxRate {RangeBegin = 0, RangeEnd = 20000, Rate = 0},
                new TaxRate {RangeBegin = 20001, RangeEnd = 40000, Rate = (decimal) 0.10},
                new TaxRate {RangeBegin = 40001, RangeEnd = 80000, Rate = (decimal) 0.20},
                new TaxRate {RangeBegin = 80001, RangeEnd = 180000, Rate = (decimal) 0.30},
                new TaxRate {RangeBegin = 180001, RangeEnd = null, Rate = (decimal) 0.40},
            };
        }


        #region Facts

        [Fact]
        public void GetTaxRatesShouldSucceed()
        {
            this.Given(x => GivenADataSource())
                .When(x => WhenCalledGetTaxRates())
                .Then(x => ThenItShouldReturnTaxRatesAsExpected())
                .BDDfy();
        }

        [Fact]
        public void GetTaxRatesShouldThrowsExceptionWithInvalidFormatData()
        {
            this.Given(x => GivenADataSourceHaveInvalidData())
                .When(x => WhenCalledGetTaxRatesWithInvalidFormatData())
                .Then(x => ThenItShouldBeSuccessful())
                .BDDfy();
        }

        #endregion



        #region Givens

        private void GivenADataSource()
        {
            _appContext = Substitute.For<IDataContext>();
            _appContext.TaxRateDataSource.ReturnsForAnyArgs(_testTaxRateData);
            _subject = new Repository(Substitute.For<ILogger>(), _appContext);
        }

        private void GivenADataSourceHaveInvalidData()
        {
            _testTaxRateData = new List<string[]>
            {
                new[] {"0", "20000", "Nil"},
                new[] {"20001", "40000", "0.10"},
                new[] {"40001", "80000", "0.20"},
                new[] {"80001", "180000", "0.30"},
                new[] {"180001", "", "0.40"}
            };
            _appContext = Substitute.For<IDataContext>();
            _appContext.TaxRateDataSource.ReturnsForAnyArgs(_testTaxRateData);
            _subject = new Repository(Substitute.For<ILogger>(), _appContext);
        }

        #endregion

        #region Whens

        private void WhenCalledGetTaxRates()
        {
            _taxRateTestResult = _subject.GeTaxRates().ToList();
        }

        private void WhenCalledGetTaxRatesWithInvalidFormatData()
        {
            Assert.Throws<FormatException>(() => _subject.GeTaxRates());
        }

        #endregion

        #region Thens

        private void ThenItShouldReturnTaxRatesAsExpected()
        {
            _taxRateTestResult.Count.ShouldBe(_taxRateExpectedResult.Count);
            for (int i = 0; i < _taxRateExpectedResult.Count; i++)
            {
                _taxRateTestResult[i].RangeBegin.ShouldBe(_taxRateExpectedResult[i].RangeBegin);
                _taxRateTestResult[i].RangeEnd.ShouldBe(_taxRateExpectedResult[i].RangeEnd);
                _taxRateTestResult[i].Rate.ShouldBe(_taxRateExpectedResult[i].Rate);

            }

        }

        private void ThenItShouldBeSuccessful()
        {
        }

        #endregion
    }
}
