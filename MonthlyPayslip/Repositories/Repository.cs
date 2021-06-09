using System;
using System.Collections.Generic;
using MonthlyPayslip.Models;
using Serilog;

namespace MonthlyPayslip.Repositories
{
    public class Repository : IRepository
    {
        private readonly IDataContext _dataContext;
        private readonly ILogger _logger;

        public Repository(ILogger logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public IEnumerable<TaxRate> GeTaxRates()
        {
            try
            {
                var taxRates = new List<TaxRate>();
                var data = _dataContext.TaxRateDataSource;
                foreach (var taxRate in data)
                {
                    if (taxRate.Length < 2)
                        continue;
                    taxRates.Add(new TaxRate
                    {
                        RangeBegin = int.Parse(taxRate[0]),
                        RangeEnd = string.IsNullOrEmpty(taxRate[1]) ? (int?)null : int.Parse(taxRate[1]),
                        Rate = taxRate.Length < 3 || string.IsNullOrEmpty(taxRate[2]) ? 0 : decimal.Parse(taxRate[2]),
                    });
                }

                return taxRates;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Unhandled Exception");
                throw;
            }
        }
    }
}
