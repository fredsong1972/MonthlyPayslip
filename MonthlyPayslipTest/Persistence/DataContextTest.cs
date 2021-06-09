using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MonthlyPayslip.Repositories;
using NSubstitute;
using MonthlyPayslip.Config;
using Serilog;
using TestStack.BDDfy;
using Xunit;

namespace PayslipAppTest.Persistence
{
    public class DataContextTest
    {
        private DataContext _subject;
        private IOptions<DataSourceSettings> _options;
        private IEnumerable<string[]> _testResult;
        private readonly IEnumerable<string[]> _expectedTaxRateData;
       
        public DataContextTest()
        {
            _expectedTaxRateData = new List<string[]>
            {
                new[] {"0", "20000", ""},
                new[] {"20001", "40000", "0.1"},
                new[] {"40001", "80000", "0.2"},
                new[] {"80001", "180000", "0.3"},
                new[] {"180001", "", "0.4"}
            };

        }

        #region Facts

        [Fact]
        public void GetTaxRateDataShouldSucceed()
        {
            this.Given(x => GivenDataSourceConfigured())
                .When(x => WhenTaxRateDataSourceGetCalled())
                .Then(x => ThenItShouldBeExpectedTaxRateData())
                .BDDfy();
        }

        #endregion

        #region Givens

        private void GivenDataSourceConfigured()
        {
            _options = Substitute.For<IOptions<DataSourceSettings>>();
            _options.Value.ReturnsForAnyArgs(new DataSourceSettings
            {
                TaxRateDataFile = "Data\\TaxRate.csv",
            });
            _subject = new DataContext(Substitute.For<ILogger>(), _options);
        }

        #endregion

        #region Whens

        private void WhenTaxRateDataSourceGetCalled()
        {
            _testResult = _subject.TaxRateDataSource;
        }

        #endregion

        #region Thens

        private void ThenItShouldBeExpectedTaxRateData()
        {
            Assert.Equal(_testResult, _expectedTaxRateData);
        }

        #endregion

    }
}
