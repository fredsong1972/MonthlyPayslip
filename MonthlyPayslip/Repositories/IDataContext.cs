using System.Collections.Generic;

namespace MonthlyPayslip.Repositories
{
    public interface IDataContext
    {
        IEnumerable<string[]> TaxRateDataSource { get; }
    }
}
