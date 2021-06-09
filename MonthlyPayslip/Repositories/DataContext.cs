using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using MonthlyPayslip.Config;
using Serilog;
namespace MonthlyPayslip.Repositories
{
    public class DataContext : IDataContext
    {
        private readonly ILogger _logger;
        private readonly DataSourceSettings _dataSourceSettings;
        public DataContext(ILogger logger, IOptions<DataSourceSettings> config)
        {
            _dataSourceSettings = config.Value;
            _logger = logger;

        }

        public IEnumerable<string[]> TaxRateDataSource
        {
            get
            {
                var contents = File.ReadAllText(_dataSourceSettings.TaxRateDataFile).Split('\n');
                var csv = from line in contents
                    select line.Replace("\r", "").Split(',').ToArray();
                return csv;
            }
        }
    }
}
