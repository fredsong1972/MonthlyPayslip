using System.Collections.Generic;
using MonthlyPayslip.Models;

namespace MonthlyPayslip.Repositories
{
    public interface IRepository
    {
        IEnumerable<TaxRate> GeTaxRates();
    }
}
